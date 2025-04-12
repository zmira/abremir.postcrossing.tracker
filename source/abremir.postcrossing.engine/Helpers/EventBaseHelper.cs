using System.Xml.Linq;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Helpers
{
    public static class EventBaseHelper
    {
        public static EventBase MapToEventBase(string rawEvent)
        {
            if (string.IsNullOrWhiteSpace(rawEvent))
            {
                return null;
            }

            return GetPostcrossingEventType(rawEvent) switch
            {
                PostcrossingEventTypeEnum.Register => ToRegisterEvent(rawEvent),
                PostcrossingEventTypeEnum.Send => ToSendEvent(rawEvent),
                PostcrossingEventTypeEnum.SignUp => ToSignUpEvent(rawEvent),
                PostcrossingEventTypeEnum.Upload => ToUploadEvent(rawEvent),
                _ => ToEventBase(rawEvent),
            };
        }

        private static PostcrossingEventTypeEnum GetPostcrossingEventType(string rawEvent)
        {
            if (string.IsNullOrWhiteSpace(rawEvent))
            {
                return PostcrossingEventTypeEnum.Unknown;
            }

            if (PostcrossingEventRegex.Register.IsMatch(rawEvent))
            {
                return PostcrossingEventTypeEnum.Register;
            }

            if (PostcrossingEventRegex.Send.IsMatch(rawEvent))
            {
                return PostcrossingEventTypeEnum.Send;
            }

            if (PostcrossingEventRegex.SignUp.IsMatch(rawEvent))
            {
                return PostcrossingEventTypeEnum.SignUp;
            }

            if (PostcrossingEventRegex.Upload.IsMatch(rawEvent))
            {
                return PostcrossingEventTypeEnum.Upload;
            }

            return PostcrossingEventTypeEnum.Unknown;
        }

        private static Register ToRegisterEvent(string rawEvent)
        {
            if (!PostcrossingEventRegex.Register.IsMatch(rawEvent))
            {
                return null;
            }

            var xelement = XElement.Parse($"<root>{rawEvent}</root>");

            return new Register
            {
                RawEvent = rawEvent,
                EventType = PostcrossingEventTypeEnum.Register,
                FromUser = xelement.ToUser(2, 2),
                User = xelement.ToUser(),
                Postcard = xelement.ToPostcard()
            };
        }

        private static Send ToSendEvent(string rawEvent)
        {
            if (!PostcrossingEventRegex.Send.IsMatch(rawEvent))
            {
                return null;
            }

            var xelement = XElement.Parse($"<root>{rawEvent}</root>");

            return new Send
            {
                RawEvent = rawEvent,
                EventType = PostcrossingEventTypeEnum.Send,
                User = xelement.ToUser(),
                ToCountry = xelement.ToCountry(2)
            };
        }

        private static SignUp ToSignUpEvent(string rawEvent)
        {
            if (!PostcrossingEventRegex.SignUp.IsMatch(rawEvent))
            {
                return null;
            }

            var xelement = XElement.Parse($"<root>{rawEvent}</root>");

            return new SignUp
            {
                RawEvent = rawEvent,
                EventType = PostcrossingEventTypeEnum.SignUp,
                User = xelement.ToUser()
            };
        }

        private static Upload ToUploadEvent(string rawEvent)
        {
            if (!PostcrossingEventRegex.Upload.IsMatch(rawEvent))
            {
                return null;
            }

            var xelement = XElement.Parse($"<root>{rawEvent}</root>");

            return new Upload
            {
                RawEvent = rawEvent,
                EventType = PostcrossingEventTypeEnum.Upload,
                User = xelement.ToUser(),
                Postcard = xelement.ToPostcard()
            };
        }

        private static EventBase ToEventBase(string rawEvent)
        {
            return new EventBase
            {
                RawEvent = rawEvent,
                EventType = PostcrossingEventTypeEnum.Unknown
            };
        }
    }
}
