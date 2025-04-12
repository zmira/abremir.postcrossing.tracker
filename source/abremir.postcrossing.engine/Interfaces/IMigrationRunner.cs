using System.Threading.Tasks;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Interfaces
{
    public interface IMigrationRunner
    {
        Task ApplyMigrations(ILiteDatabaseAsync liteDatabase);
    }
}
