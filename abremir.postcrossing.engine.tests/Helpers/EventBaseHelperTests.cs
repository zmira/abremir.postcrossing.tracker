using abremir.postcrossing.engine.Helpers;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Helpers
{
    [TestClass]
    public class EventBaseHelperTests
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void MapToEventBase_InvalidRawEvent_ReturnsNull(string rawEvent)
        {
            var result = EventBaseHelper.MapToEventBase(rawEvent);

            Check.That(result).IsNull();
        }

        [TestMethod]
        public void MapToEventBase_RegisterEvent_ReturnsRegister()
        {
            const string toUser = "to_user";
            const string fromUser = "from_user";
            const string postcardId = "postcard_id";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{toUser}"">{toUser}</a> received a <a href=""/postcards/{postcardId}"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{fromUser}"">{fromUser}</a>";

            var result = EventBaseHelper.MapToEventBase(rawEvent) as Register;

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<Register>();
            Check.That(result.EventType).IsEqualTo(PostcrossingEventTypeEnum.Register);
            Check.That(result.User.Name).IsEqualTo(toUser);
            Check.That(result.FromUser.Name).IsEqualTo(fromUser);
            Check.That(result.Postcard.PostcardId).IsEqualTo(postcardId);
        }

        [TestMethod]
        public void MapToEventBase_SendEvent_ReturnsSend()
        {
            const string fromUser = "from_user";
            const string toCountry = "to_country";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">{fromUser}</a> sent a postcard to <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">{toCountry}</a>";

            var result = EventBaseHelper.MapToEventBase(rawEvent) as Send;

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<Send>();
            Check.That(result.EventType).IsEqualTo(PostcrossingEventTypeEnum.Send);
            Check.That(result.User.Name).IsEqualTo(fromUser);
            Check.That(result.ToCountry.Name).IsEqualTo(toCountry);
        }

        [TestMethod]
        public void MapToEventBase_SignUpEvent_ReturnsSignUp()
        {
            const string userName = "user_name";
            var rawEvent = $@"<a href =""/user/user"">{userName}</a> from <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up";

            var result = EventBaseHelper.MapToEventBase(rawEvent) as SignUp;

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<SignUp>();
            Check.That(result.EventType).IsEqualTo(PostcrossingEventTypeEnum.SignUp);
            Check.That(result.User.Name).IsEqualTo(userName);
        }

        [TestMethod]
        public void MapToEventBase_UploadEvent_ReturnsUpload()
        {
            const string userName = "user_name";
            const string postcardId = "postcard_id";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">{userName}</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/{postcardId}"">card</a>";

            var result = EventBaseHelper.MapToEventBase(rawEvent) as Upload;

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<Upload>();
            Check.That(result.EventType).IsEqualTo(PostcrossingEventTypeEnum.Upload);
            Check.That(result.User.Name).IsEqualTo(userName);
            Check.That(result.Postcard.PostcardId).IsEqualTo(postcardId);
        }

        [TestMethod]
        public void MapToEventBase_UnknownEvent_ReturnsEventBase()
        {
            const string rawEvent = @"Account closed for <a href=""/user/user"">user</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>";

            var result = EventBaseHelper.MapToEventBase(rawEvent);

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<EventBase>();
            Check.That(result.EventType).IsEqualTo(PostcrossingEventTypeEnum.Unknown);
        }
    }
}
