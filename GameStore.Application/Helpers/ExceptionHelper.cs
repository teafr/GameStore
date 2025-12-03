using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Application.Helpers;

public static class ExceptionHelper
{
    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number == 2601 || sqlEx.Number == 2627;
        }
        return ex.InnerException?.Message.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
