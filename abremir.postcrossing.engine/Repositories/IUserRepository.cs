using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<IEnumerable<User>> All();
        Task<User> Get(Expression<Func<User, bool>> predicate);
        Task<User> GetOrAdd(User user);
    }
}
