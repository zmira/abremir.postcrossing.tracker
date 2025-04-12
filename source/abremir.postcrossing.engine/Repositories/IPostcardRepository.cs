using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IPostcardRepository
    {
        Task<Postcard> Add(Postcard postcard);
        Task<IEnumerable<Postcard>> All();
        Task<Postcard> Get(Expression<Func<Postcard, bool>> predicate);
        Task<Postcard> GetOrAdd(Postcard postcard);
    }
}
