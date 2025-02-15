using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using abremir.postcrossing.engine.Models;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IPostcardRepository
    {
        Postcard Add(Postcard postcard);
        IEnumerable<Postcard> All();
        Postcard Get(Expression<Func<Postcard, bool>> predicate);
        Postcard GetOrAdd(Postcard postcard);
    }
}
