using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IEventComposer
    {
        T ComposeEvent<T>(EventBase postcrossingEvent) where T : EventBase;
    }
}
