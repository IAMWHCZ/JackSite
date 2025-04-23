namespace JackSite.Common.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>;