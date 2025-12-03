namespace GameStore.Domain.Exceptions.DatabaseExceptions;

public class ObjectNotFoundException(string objectType, object key) 
    : Exception($"{objectType} with '{key}' identifier was not found.") { }
