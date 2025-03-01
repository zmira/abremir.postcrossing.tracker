using System.Linq;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Helpers;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Repositories;
using abremir.postcrossing.engine.tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Repositories
{
    [TestClass]
    public class EventRepositoryTests : DataTestsBase
    {
        private readonly EventRepository _eventRepository;

        public EventRepositoryTests()
        {
            var countryRepository = new CountryRepository(MemoryRepositoryService);
            var userRepository = new UserRepository(MemoryRepositoryService);
            var postcardRepository = new PostcardRepository(MemoryRepositoryService);
            var eventComposer = new EventComposer(countryRepository, userRepository, postcardRepository);
            _eventRepository = new EventRepository(MemoryRepositoryService, eventComposer);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Add_NoRawEvent_DoesNotInsert(string rawEvent)
        {
            var postcrossingEvent = new EventBase
            {
                RawEvent = rawEvent
            };

            var result = await _eventRepository.Add(postcrossingEvent);

            Check.That(result).IsNull();
            Check.ThatCode(MemoryRepositoryService.GetRepository().Database.GetCollection(PostcrossingTrackerConstants.EventCollectionName).CountAsync).WhichResult().Is(0);
        }

        [DataTestMethod]
        [DataRow(@"Account closed for <a href=""/user/user"">user</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>")]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/postcard"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>")]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> sent a postcard to <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>")]
        [DataRow(@"<a href =""/user/user"">user</a> from <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up")]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/postcard"">card</a>")]
        public async Task Add_ValidEvent_Inserts(string rawEvent)
        {
            var postcrossingEvent = EventBaseHelper.MapToEventBase(rawEvent);

            var result = await _eventRepository.Add(postcrossingEvent);

            Check.That(result).Not.IsNull();
            Check.ThatCode(MemoryRepositoryService.GetRepository().Database.GetCollection(PostcrossingTrackerConstants.EventCollectionName).CountAsync).WhichResult().IsEqualTo(1);
        }

        [TestMethod]
        public async Task Get_EventDoesNotExist_ReturnsNull()
        {
            var result = await _eventRepository.Get<SignUp>(5);

            Check.That(result).IsNull();
        }

        [TestMethod]
        public async Task Get_EventExists_ReturnsEvent()
        {
            var postcrossingEvent = EventBaseHelper.MapToEventBase(@"Account closed for <a href=""/user/user"">user</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>");
            postcrossingEvent.EventId = 999;

            await _eventRepository.Add(postcrossingEvent);

            var result = await _eventRepository.Get<EventBase>(postcrossingEvent.EventId);

            Check.That(result).IsNotNull();
        }

        [TestMethod]
        public async Task FindEventsWithIdGreaterThan_()
        {
            var postcrossingEvent1 = EventBaseHelper.MapToEventBase(@"Account closed for <a href=""/user/user1"">user1</a> from  <i title=""country"" class=""flag flag-YY""></i> <a href=""/country/YY"">countryYY</a>");
            postcrossingEvent1.EventId = 998;
            var postcrossingEvent2 = EventBaseHelper.MapToEventBase(@"Account closed for <a href=""/user/user2"">user2</a> from  <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">countryXX</a>");
            postcrossingEvent2.EventId = 999;

            await _eventRepository.Add(postcrossingEvent1);
            await _eventRepository.Add(postcrossingEvent2);

            var result = await _eventRepository.FindEventsWithIdGreaterThan<EventBase>(postcrossingEvent1.EventId);

            Check.That(result).Not.IsEmpty();
            Check.That(result).CountIs(1);
            Check.That(result.First()).HasFieldsWithSameValues(postcrossingEvent2);
        }
    }
}
