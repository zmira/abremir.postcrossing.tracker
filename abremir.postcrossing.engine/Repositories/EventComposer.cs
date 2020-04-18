using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Repositories
{
    public class EventComposer : IEventComposer
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostcardRepository _postcardRepository;

        public EventComposer(
            ICountryRepository countryRepository,
            IUserRepository userRepository,
            IPostcardRepository postcardRepository)
        {
            _countryRepository = countryRepository;
            _userRepository = userRepository;
            _postcardRepository = postcardRepository;
        }

        public T ComposeEvent<T>(EventBase postcrossingEvent) where T : EventBase
        {
            switch (postcrossingEvent.EventType)
            {
                case PostcrossingEventTypeEnum.Register:
                    return ComposeRegister(postcrossingEvent as Register) as T;
                case PostcrossingEventTypeEnum.Send:
                    return ComposeSend(postcrossingEvent as Send) as T;
                case PostcrossingEventTypeEnum.SignUp:
                    return ComposeSignUp(postcrossingEvent as SignUp) as T;
                case PostcrossingEventTypeEnum.Upload:
                    return ComposeUpload(postcrossingEvent as Upload) as T;
                default:
                    return null;
            }
        }

        private Register ComposeRegister(Register postcrossingEvent)
        {
            postcrossingEvent.User.Country = _countryRepository.GetOrAdd(postcrossingEvent.User.Country);
            postcrossingEvent.User = _userRepository.GetOrAdd(postcrossingEvent.User);
            postcrossingEvent.Postcard.Country = _countryRepository.GetOrAdd(postcrossingEvent.Postcard.Country);
            postcrossingEvent.Postcard = _postcardRepository.GetOrAdd(postcrossingEvent.Postcard);
            postcrossingEvent.FromUser.Country = _countryRepository.GetOrAdd(postcrossingEvent.FromUser.Country);
            postcrossingEvent.FromUser = _userRepository.GetOrAdd(postcrossingEvent.FromUser);

            return postcrossingEvent;
        }

        private Send ComposeSend(Send postcrossingEvent)
        {
            postcrossingEvent.User.Country = _countryRepository.GetOrAdd(postcrossingEvent.User.Country);
            postcrossingEvent.User = _userRepository.GetOrAdd(postcrossingEvent.User);
            postcrossingEvent.ToCountry = _countryRepository.GetOrAdd(postcrossingEvent.ToCountry);

            return postcrossingEvent;
        }

        private SignUp ComposeSignUp(SignUp postcrossingEvent)
        {
            postcrossingEvent.User.Country = _countryRepository.GetOrAdd(postcrossingEvent.User.Country);
            postcrossingEvent.User = _userRepository.GetOrAdd(postcrossingEvent.User);

            return postcrossingEvent;
        }

        private Upload ComposeUpload(Upload postcrossingEvent)
        {
            postcrossingEvent.User.Country = _countryRepository.GetOrAdd(postcrossingEvent.User.Country);
            postcrossingEvent.User = _userRepository.GetOrAdd(postcrossingEvent.User);
            postcrossingEvent.Postcard.Country = _countryRepository.GetOrAdd(postcrossingEvent.Postcard.Country);
            postcrossingEvent.Postcard = _postcardRepository.GetOrAdd(postcrossingEvent.Postcard);

            return postcrossingEvent;
        }
    }
}
