namespace ires.Domain.Exceptions
{
    public class EntityNotFoundException(string message = "Request Entity not found") : Exception(message)
    {
    }
}
