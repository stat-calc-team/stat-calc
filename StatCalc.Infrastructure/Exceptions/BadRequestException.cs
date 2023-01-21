namespace StatCalc.Infrastructure.Exceptions;

public class BadRequestException : Exception
{
    private const string DefaultMessage = "Bad request";
    public BadRequestException(): base(DefaultMessage) { }
    public BadRequestException(string message): base(message) { }
}
