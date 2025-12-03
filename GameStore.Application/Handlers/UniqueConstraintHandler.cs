using GameStore.Application.Helpers;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Application.Handlers;

public static class UniqueConstraintHandler
{
    public static async Task<TEntity> HandleAsync<TEntity>(Func<Task<TEntity>> action, string property, object value)
    {
        try
        {
            return await action();
        }
        catch (DbUpdateException ex) when (ExceptionHelper.IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateObjectException<TEntity>(property, value);
        }
    }
}
