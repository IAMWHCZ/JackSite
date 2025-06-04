namespace JackSite.Authentication.Exceptions;

public class EmailException : Exception
{
    public string EmailAddress { get; }
    
    public EmailException() : base("发生电子邮件错误")
    {
    }
    
    public EmailException(string message) : base(message)
    {
    }
    
    public EmailException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public EmailException(string message, string emailAddress) : base(message)
    {
        EmailAddress = emailAddress;
    }
    
    public EmailException(string message, string emailAddress, Exception innerException) : base(message, innerException)
    {
        EmailAddress = emailAddress;
    }
}