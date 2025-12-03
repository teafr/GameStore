namespace GameStore.Domain.Exceptions.FileExceptions;

public class FileGenerationException(string message, Exception? inner = null) : Exception(message, inner) { }
