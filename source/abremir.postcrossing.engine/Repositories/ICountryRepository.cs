using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models;

namespace abremir.postcrossing.engine.Repositories
{
    public interface ICountryRepository
    {
        Task<Country> Add(Country country);
        Task<IEnumerable<Country>> All();
        Task<Country> Get(Expression<Func<Country, bool>> predicate);
        Task<Country> GetOrAdd(Country country);
    }
}
