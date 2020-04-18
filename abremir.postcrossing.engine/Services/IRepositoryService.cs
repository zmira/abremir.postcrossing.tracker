using LiteDB;

namespace abremir.postcrossing.engine.Services
{
    public interface IRepositoryService
    {
        ILiteRepository GetRepository();
        long CompactRepository();
    }
}
