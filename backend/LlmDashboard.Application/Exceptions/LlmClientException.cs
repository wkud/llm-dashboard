namespace LlmDashboard.Application.Exceptions;

public class LlmClientException : Exception
{
    public LlmClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
    public LlmClientException(string message) : base(message)
    {
    }
}