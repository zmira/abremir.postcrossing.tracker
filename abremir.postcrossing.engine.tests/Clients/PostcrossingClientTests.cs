using abremir.postcrossing.engine.Clients;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NFluent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace abremir.postcrossing.engine.tests.Clients
{
    [TestClass]
    public class PostcrossingClientTests
    {
        private readonly HttpTest _httpTest;
        private readonly IPostcrossingClient _postcrossingClient;

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
            var eventId = 1;
            var toUser = "to_user";
            var fromUser = "from_user";
            var postcardId = "postcard_id";
            var rawEvent = $@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{toUser}"">{toUser}</a> received a <a href=""/postcards/{postcardId}"">postcard</a> from <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{fromUser}"">{fromUser}</a>";
            var returnValue = new List<string> { eventId.ToString(), rawEvent };

            _httpTest.RespondWith($"[{JsonConvert.SerializeObject(returnValue)}]");

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
            var eventId = 2;
            var fromUser = "from_user";
            var toCountry = "to_country";
            var rawEvent = $@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">{fromUser}</a> sent a postcard to <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/country/XX"">{toCountry}</a>";
            var returnValue = new List<string> { eventId.ToString(), rawEvent };

            _httpTest.RespondWith($"[{JsonConvert.SerializeObject(returnValue)}]");

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
            var eventId = 3;
            var userName = "user_name";
            var rawEvent = $@"<a href=""/user/user"">{userName}</a> from <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up";
            var returnValue = new List<string> { eventId.ToString(), rawEvent };

            _httpTest.RespondWith($"[{JsonConvert.SerializeObject(returnValue)}]");

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
            var eventId = 4;
            var userName = "user_name";
            var postcardId = "postcard_id";
            var rawEvent = $@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">{userName}</a> uploaded postcard <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/postcards/{postcardId}"">card</a>";
            var returnValue = new List<string> { eventId.ToString(), rawEvent };

            _httpTest.RespondWith($"[{JsonConvert.SerializeObject(returnValue)}]");

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
            var eventId = 5;
            var rawEvent = @"Account closed for <a href=""/user/user"">user</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>";
            var returnValue = new List<string> { eventId.ToString(), rawEvent };

            _httpTest.RespondWith($"[{JsonConvert.SerializeObject(returnValue)}]");

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
