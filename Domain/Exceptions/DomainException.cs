namespace Domain.Exceptions;

public class DomainException(string message) : Exception(message);
public class GameAlreadyCompletedException() : DomainException("The game is already completed");
public class CellAlreadyMarkedException() : DomainException("This cell is already marked");
public class BadPlayerException() : DomainException("This is not your turn");
public class IncorrectMarksException() : DomainException("These marks are not correct");
