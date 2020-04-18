using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace abremir.postcrossing.engine.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IRepositoryService _repositoryService;

        public CountryRepository(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public Country Add(Country country)
        {
            if (country == null
                || string.IsNullOrWhiteSpace(country.Name)
                || string.IsNullOrWhiteSpace(country.Code))
            {
                return null;
            }

            country.TrimAllStrings();

            using var repository = _repositoryService.GetRepository();

            repository.Insert(country, PostcrossingTrackerConstants.CountryCollectionName);

            return country;
        }

        public IEnumerable<Country> All()
        {
            using var repository = _repositoryService.GetRepository();

            return repository.Fetch<Country>("1 = 1", PostcrossingTrackerConstants.CountryCollectionName);
        }

        public Country Get(Expression<Func<Country, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return repository.FirstOrDefault(predicate, PostcrossingTrackerConstants.CountryCollectionName);
        }

        public Country GetOrAdd(Country country)
        {
            return Get(countryModel => countryModel.Code == country.Code) ?? Add(country);
        }
    }
}
