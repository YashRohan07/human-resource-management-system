namespace HRMS.API.Exceptions;

// Thrown when requested data is missing
public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, 404)
    {
    }
}