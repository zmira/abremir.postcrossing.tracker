using abremir.postcrossing.engine.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
