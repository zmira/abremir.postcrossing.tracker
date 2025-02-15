using System;
using System.Linq;
using abremir.postcrossing.engine.Models.Enumerations;

namespace abremir.postcrossing.engine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class AssociatedEventType(PostcrossingEventTypeEnum eventType) : Attribute
    {
        public PostcrossingEventTypeEnum EventType { get; } = eventType;

        public static PostcrossingEventTypeEnum? GetAssociatedEventType<T>()
        {
            var objectType = typeof(T);
            var associatedEventType = objectType.GetCustomAttributes(typeof(AssociatedEventType), false);

            return ((AssociatedEventType)associatedEventType.FirstOrDefault())?.EventType;
        }
    }
}
