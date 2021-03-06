﻿using abremir.postcrossing.engine.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace abremir.postcrossing.engine.Repositories
{
    public interface ICountryRepository
    {
        Country Add(Country country);
        IEnumerable<Country> All();
        Country Get(Expression<Func<Country, bool>> predicate);
        Country GetOrAdd(Country country);
    }
}
