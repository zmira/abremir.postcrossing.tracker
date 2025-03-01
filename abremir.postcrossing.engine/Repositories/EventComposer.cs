using System.Threading.Tasks;
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

        public async Task<T> ComposeEvent<T>(EventBase postcrossingEvent) where T : EventBase
        {
            return postcrossingEvent.EventType switch
            {
                PostcrossingEventTypeEnum.Register => (await ComposeRegister(postcrossingEvent as Register).ConfigureAwait(false)) as T,
                PostcrossingEventTypeEnum.Send => (await ComposeSend(postcrossingEvent as Send).ConfigureAwait(false)) as T,
                PostcrossingEventTypeEnum.SignUp => (await ComposeSignUp(postcrossingEvent as SignUp).ConfigureAwait(false)) as T,
                PostcrossingEventTypeEnum.Upload => (await ComposeUpload(postcrossingEvent as Upload).ConfigureAwait(false)) as T,
                _ => null,
            };
        }

        private async Task<Register> ComposeRegister(Register postcrossingEvent)
        {
            postcrossingEvent.User.Country = await _countryRepository.GetOrAdd(postcrossingEvent.User.Country).ConfigureAwait(false);
            postcrossingEvent.User = await _userRepository.GetOrAdd(postcrossingEvent.User).ConfigureAwait(false);
            postcrossingEvent.Postcard.Country = await _countryRepository.GetOrAdd(postcrossingEvent.Postcard.Country).ConfigureAwait(false);
            postcrossingEvent.Postcard = await _postcardRepository.GetOrAdd(postcrossingEvent.Postcard).ConfigureAwait(false);
            postcrossingEvent.FromUser.Country = await _countryRepository.GetOrAdd(postcrossingEvent.FromUser.Country).ConfigureAwait(false);
            postcrossingEvent.FromUser = await _userRepository.GetOrAdd(postcrossingEvent.FromUser).ConfigureAwait(false);

            return postcrossingEvent;
        }

        private async Task<Send> ComposeSend(Send postcrossingEvent)
        {
            postcrossingEvent.User.Country = await _countryRepository.GetOrAdd(postcrossingEvent.User.Country).ConfigureAwait(false);
            postcrossingEvent.User = await _userRepository.GetOrAdd(postcrossingEvent.User).ConfigureAwait(false);
            postcrossingEvent.ToCountry = await _countryRepository.GetOrAdd(postcrossingEvent.ToCountry).ConfigureAwait(false);

            return postcrossingEvent;
        }

        private async Task<SignUp> ComposeSignUp(SignUp postcrossingEvent)
        {
            postcrossingEvent.User.Country = await _countryRepository.GetOrAdd(postcrossingEvent.User.Country).ConfigureAwait(false);
            postcrossingEvent.User = await _userRepository.GetOrAdd(postcrossingEvent.User).ConfigureAwait(false);

            return postcrossingEvent;
        }

        private async Task<Upload> ComposeUpload(Upload postcrossingEvent)
        {
            postcrossingEvent.User.Country = await _countryRepository.GetOrAdd(postcrossingEvent.User.Country).ConfigureAwait(false);
            postcrossingEvent.User = await _userRepository.GetOrAdd(postcrossingEvent.User).ConfigureAwait(false);
            postcrossingEvent.Postcard.Country = await _countryRepository.GetOrAdd(postcrossingEvent.Postcard.Country).ConfigureAwait(false);
            postcrossingEvent.Postcard = await _postcardRepository.GetOrAdd(postcrossingEvent.Postcard).ConfigureAwait(false);

            return postcrossingEvent;
        }
    }
}
