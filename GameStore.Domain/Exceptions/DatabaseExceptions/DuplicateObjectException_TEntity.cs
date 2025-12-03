namespace GameStore.Domain.Exceptions.DatabaseExceptions;

public class DuplicateObjectException<TEntity>(string property, object value) 
    : DuplicateObjectException($"{typeof(TEntity).Name} with {property} '{value}' already exists.") { }
