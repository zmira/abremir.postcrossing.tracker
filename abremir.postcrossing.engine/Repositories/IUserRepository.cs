using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using abremir.postcrossing.engine.Models;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IUserRepository
    {
        User Add(User user);
        IEnumerable<User> All();
        User Get(Expression<Func<User, bool>> predicate);
        User GetOrAdd(User user);
    }
}
