namespace GameStore.Domain.Exceptions.DatabaseExceptions;

public class DuplicateObjectException(string message) : Exception(message) { }
