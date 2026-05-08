namespace HRMS.API.Exceptions;

// Used when requested data is not found
public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, 404)
    {
    }
}