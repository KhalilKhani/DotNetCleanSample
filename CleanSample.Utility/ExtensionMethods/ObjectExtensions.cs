namespace CleanSample.Utility.ExtensionMethods;

public static class ObjectExtensions
{
    public static bool HasSameValuesAs<T>(
        this T actualObject, 
        T expectedObject, 
        List<string>? excludedProperties = null)
    {
        var expectedProperties = expectedObject.GetType().GetProperties();
        var actualProperties = actualObject.GetType().GetProperties();

        foreach (var property in expectedProperties)
        {
            // Check if the property is in the exclusion list
            if (excludedProperties != null && excludedProperties.Contains(property.Name))
                continue;

            var expectedValue = property.GetValue(expectedObject);
            var actualValue = actualProperties.First(p => p.Name == property.Name).GetValue(actualObject);

            if (!Equals(expectedValue, actualValue))
                return false;
        }
        return true;
    }
}

