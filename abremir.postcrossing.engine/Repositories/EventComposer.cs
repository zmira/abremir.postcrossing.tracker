using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Repositories
{
    public class EventComposer(
        ICountryRepository countryRepository,
        IUserRepository userRepository,
        IPostcardRepository postcardRepository) : IEventComposer
    {
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPostcardRepository _postcardRepository = postcardRepository;

        public T ComposeEvent<T>(EventBase postcrossingEvent) where T : EventBase
        {
            return postcrossingEvent.EventType switch
            {
                PostcrossingEventTypeEnum.Register => ComposeRegister(postcrossingEvent as Register) as T,
                PostcrossingEventTypeEnum.Send => ComposeSend(postcrossingEvent as Send) as T,
                PostcrossingEventTypeEnum.SignUp => ComposeSignUp(postcrossingEvent as SignUp) as T,
                PostcrossingEventTypeEnum.Upload => ComposeUpload(postcrossingEvent as Upload) as T,
                _ => null,
            };
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
