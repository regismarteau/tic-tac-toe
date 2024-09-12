namespace Infrastructure.Exceptions
{
    public class NotFoundException(string message) : Exception(message);

    public class GameNotFoundException() : NotFoundException("Game not found");
}
