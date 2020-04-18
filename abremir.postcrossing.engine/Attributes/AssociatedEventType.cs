using abremir.postcrossing.engine.Models.Enumerations;
using System;
using System.Linq;

namespace abremir.postcrossing.engine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class AssociatedEventType : Attribute
    {
        public AssociatedEventType(PostcrossingEventTypeEnum eventType)
        {
            EventType = eventType;
        }

        public PostcrossingEventTypeEnum EventType { get; }

        public static PostcrossingEventTypeEnum? GetAssociatedEventType<T>()
        {
            var objectType = typeof(T);
            var associatedEventType = objectType.GetCustomAttributes(typeof(AssociatedEventType), false);

            return ((AssociatedEventType)associatedEventType.FirstOrDefault())?.EventType;
        }
    }
}
