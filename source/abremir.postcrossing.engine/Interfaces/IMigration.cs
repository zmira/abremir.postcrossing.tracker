using System.Threading.Tasks;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Interfaces
{
    public interface IMigration
    {
        int MigrationId { get; }

        Task Apply(ILiteDatabaseAsync liteDatabase);
    }
}
