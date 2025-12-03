namespace GameStore.Domain.Exceptions.DatabaseExceptions;

public class InvalidReferenceException(string entity, IEnumerable<Guid> ids) 
    : Exception($"{entity}(s) with the following ID(s) do not exist: {string.Join(", ", ids)}") { }
