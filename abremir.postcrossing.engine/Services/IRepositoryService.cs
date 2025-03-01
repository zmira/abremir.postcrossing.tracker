using LiteDB.Async;

namespace abremir.postcrossing.engine.Services
{
    public interface IRepositoryService
    {
        ILiteRepositoryAsync GetRepository();
    }
}
