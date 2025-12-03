namespace GameStore.Domain.Exceptions.DatabaseExceptions;

public class ParentGenreNotFoundException(string parentName) : Exception($"Parent genre '{parentName}' not found.") { }