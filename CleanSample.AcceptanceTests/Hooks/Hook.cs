// namespace CleanSample.AcceptanceTests.Hooks;
//
// [Binding]
// public class Hooks
// {
//     private const string ApiProcessKey = "ApiProcess";
//     private readonly string _apiProjectPath;
//     private readonly ScenarioContext _scenarioContext;
//     private readonly ApiClientDriver _apiClientDriver;
//
//     public Hooks(ScenarioContext scenarioContext, ApiClientDriver apiClientDriver)
//     {
//         _scenarioContext = scenarioContext;
//         _apiClientDriver = apiClientDriver;
//
//         // Set the path to your API project
//         var solutionRoot = FindSolutionRoot(AppDomain.CurrentDomain.BaseDirectory);
//         _apiProjectPath = Path.Combine(solutionRoot, "CleanSample.Presentation", "Server");
//     }
//
//     [BeforeScenario(Order = 0)]
//     public void BeforeScenario()
//     {
//         // We use in-memory database for TestMode. This way we reset the database before every scenario
//         StopRunningApi();
//         StartApi();
//     }
//
//
//     [AfterScenario]
//     public void AfterScenario()
//     {
//         StopRunningApi();
//     }
//
//     private void StopRunningApi()
//     {
//         if (_scenarioContext.ContainsKey(ApiProcessKey))
//         {
//             var runningProcess = _scenarioContext.Get<Process>(ApiProcessKey);
//             runningProcess?.Kill();
//             _scenarioContext.Remove(ApiProcessKey);
//         }
//     }
//
//
//     private void StartApi()
//     {
//         var processStartInfo = new ProcessStartInfo
//         {
//             FileName = "dotnet",
//             Arguments = "run --launch-profile \"TestMode\"",
//             RedirectStandardOutput = true,
//             UseShellExecute = false,
//             CreateNoWindow = true,
//             WorkingDirectory = _apiProjectPath
//         };
//
//         var process = Process.Start(processStartInfo);
//         _scenarioContext.Set(process, ApiProcessKey);
//
//         // Wait for API to be fully started
//         WaitForServerStart();
//     }
//
//     private void WaitForServerStart()
//     {
//         var startTime = DateTime.Now;
//         const int timeoutSeconds = 30;
//
//         while (true)
//         {
//             if (DateTime.Now.Subtract(startTime).TotalSeconds > timeoutSeconds)
//                 throw new TimeoutException("Timeout waiting for server to start");
//
//             var response = _apiClientDriver.GetAllCustomersAsync().Result;
//             if (response.IsSuccess)
//                 break;
//
//             Thread.Sleep(TimeSpan.FromSeconds(1));
//         }
//     }
//
//
//     public static string FindSolutionRoot(string startDirectory)
//     {
//         DirectoryInfo? directory = new(startDirectory);
//
//         while (directory != null)
//         {
//             var solutionFiles = directory.GetFiles("*.sln");
//             if (solutionFiles.Length > 0)
//                 return directory.FullName;
//
//             directory = directory.Parent;
//         }
//
//         throw new Exception("Solution root could not be found.");
//     }
// }