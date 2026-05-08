namespace HRMS.API.Exceptions;

// Thrown when conflicting data is found
public class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, 409)
    {
    }
}