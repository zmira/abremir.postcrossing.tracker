using System.Threading.Tasks;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IEventComposer
    {
        Task<T> ComposeEvent<T>(EventBase postcrossingEvent) where T : EventBase;
    }
}
