using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;

namespace abremir.postcrossing.engine.Repositories
{
    public class CountryRepository(IRepositoryService repositoryService) : ICountryRepository
    {
        private readonly IRepositoryService _repositoryService = repositoryService;

        public async Task<Country> Add(Country country)
        {
            if (country == null
                || string.IsNullOrWhiteSpace(country.Name)
                || string.IsNullOrWhiteSpace(country.Code))
            {
                return null;
            }

            using var repository = _repositoryService.GetRepository();

            await repository.InsertAsync(country, PostcrossingTrackerConstants.CountryCollectionName).ConfigureAwait(false);

            return country;
        }

        public async Task<IEnumerable<Country>> All()
        {
            using var repository = _repositoryService.GetRepository();

            return await repository.FetchAsync<Country>("1 = 1", PostcrossingTrackerConstants.CountryCollectionName).ConfigureAwait(false);
        }

        public async Task<Country> Get(Expression<Func<Country, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return await repository.FirstOrDefaultAsync(predicate, PostcrossingTrackerConstants.CountryCollectionName).ConfigureAwait(false);
        }

        public async Task<Country> GetOrAdd(Country country)
        {
            return await Get(countryModel => countryModel.Code == country.Code).ConfigureAwait(false) ?? await Add(country).ConfigureAwait(false);
        }
    }
}
