using System.Text.Json.Nodes;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Clients;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Clients
{
    [TestClass]
    public class PostcrossingClientTests
    {
        private readonly HttpTest _httpTest;
        private readonly PostcrossingClient _postcrossingClient;

        public PostcrossingClientTests()
        {
            _httpTest = new HttpTest();
            _postcrossingClient = new PostcrossingClient();
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_NoNewEvents_ReturnsEmptyEnumerable()
        {
            _httpTest.RespondWith("[]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.IsEmpty();
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_RegisterEvent_ReturnsEnumerableWithRegisterEvent()
        {
            const long eventId = 1;
            const string toUser = "to_user";
            const string fromUser = "from_user";
            const string postcardId = "postcard_id";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{toUser}"">{toUser}</a> received a <a href=""/postcards/{postcardId}"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{fromUser}"">{fromUser}</a>";
            var returnValue = new JsonArray { eventId, rawEvent };

            _httpTest.RespondWith($"[{returnValue.ToJsonString()}]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.ContainsOnlyInstanceOfType(typeof(Register))
                .And.HasOneElementOnly()
                .And.ContainsOnlyElementsThatMatch(element => element.EventId == eventId)
                .And.ContainsOnlyElementsThatMatch(element => element.EventType == PostcrossingEventTypeEnum.Register);
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_SendEvent_ReturnsEnumerableWithSendEvent()
        {
            const long eventId = 2;
            const string fromUser = "from_user";
            const string toCountry = "to_country";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">{fromUser}</a> sent a postcard to <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">{toCountry}</a>";
            var returnValue = new JsonArray { eventId, rawEvent };

            _httpTest.RespondWith($"[{returnValue.ToJsonString()}]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.ContainsOnlyInstanceOfType(typeof(Send))
                .And.HasOneElementOnly()
                .And.ContainsOnlyElementsThatMatch(element => element.EventId == eventId)
                .And.ContainsOnlyElementsThatMatch(element => element.EventType == PostcrossingEventTypeEnum.Send);
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_SignUpEvent_ReturnsEnumerableWithSignUpEvent()
        {
            const long eventId = 3;
            const string userName = "user_name";
            var rawEvent = $@"<a href =""/user/user"">{userName}</a> from <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up";
            var returnValue = new JsonArray { eventId, rawEvent };

            _httpTest.RespondWith($"[{returnValue.ToJsonString()}]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.ContainsOnlyInstanceOfType(typeof(SignUp))
                .And.HasOneElementOnly()
                .And.ContainsOnlyElementsThatMatch(element => element.EventId == eventId)
                .And.ContainsOnlyElementsThatMatch(element => element.EventType == PostcrossingEventTypeEnum.SignUp);
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_UploadEvent_ReturnsEnumerableWithUploadEvent()
        {
            const long eventId = 4;
            const string userName = "user_name";
            const string postcardId = "postcard_id";
            var rawEvent = $@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">{userName}</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/{postcardId}"">card</a>";
            var returnValue = new JsonArray { eventId, rawEvent };

            _httpTest.RespondWith($"[{returnValue.ToJsonString()}]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.ContainsOnlyInstanceOfType(typeof(Upload))
                .And.HasOneElementOnly()
                .And.ContainsOnlyElementsThatMatch(element => element.EventId == eventId)
                .And.ContainsOnlyElementsThatMatch(element => element.EventType == PostcrossingEventTypeEnum.Upload);
        }

        [TestMethod]
        public async Task GetPostcrossingEventsAsync_UnknownEvent_ReturnsEnumerableWithEventBase()
        {
            const long eventId = 5;
            const string rawEvent = @"Account closed for <a href=""/user/user"">user</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>";
            var returnValue = new JsonArray { eventId, rawEvent };

            _httpTest.RespondWith($"[{returnValue.ToJsonString()}]");

            var result = await _postcrossingClient.GetPostcrossingEventsAsync();

            Check.That(result)
                .IsNotNull()
                .And.ContainsOnlyInstanceOfType(typeof(EventBase))
                .And.HasOneElementOnly()
                .And.ContainsOnlyElementsThatMatch(element => element.EventId == eventId)
                .And.ContainsOnlyElementsThatMatch(element => element.EventType == PostcrossingEventTypeEnum.Unknown);
        }
    }
}
