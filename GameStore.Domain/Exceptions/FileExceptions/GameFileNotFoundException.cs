namespace GameStore.Domain.Exceptions.FileExceptions;

public class GameFileNotFoundException(string key) : Exception($"Game file for key '{key}' was not found.") { }