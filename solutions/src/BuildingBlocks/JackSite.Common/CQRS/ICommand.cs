namespace JackSite.Common.CQRS;

public interface ICommand<TResult> : IRequest<TResult>;

public interface ICommand : ICommand<Result>;

