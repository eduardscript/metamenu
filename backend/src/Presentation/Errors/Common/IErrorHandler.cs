namespace Presentation.Errors.Common;

public interface IErrorHandler<in TException> where TException : Exception
{
    IError HandleError(IError error, TException exception);
}