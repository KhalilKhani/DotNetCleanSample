using CleanSample.Utility;

namespace CleanSample.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{

}


public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}