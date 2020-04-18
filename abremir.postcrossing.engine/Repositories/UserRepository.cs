using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace abremir.postcrossing.engine.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepositoryService _repositoryService;

        public UserRepository(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public User Add(User user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.Name)
                || user.Country == null
                || user.Country.Id == 0)
            {
                return null;
            }

            user.TrimAllStrings();

            using var repository = _repositoryService.GetRepository();

            repository.Insert(user, PostcrossingTrackerConstants.UserCollectionName);

            return user;
        }

        public IEnumerable<User> All()
        {
            using var repository = _repositoryService.GetRepository();

            return GetQueryable(repository)
                .ToList();
        }

        public User Get(Expression<Func<User, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return GetQueryable(repository)
                .Where(predicate)
                .FirstOrDefault();
        }

        public User GetOrAdd(User user)
        {
            return Get(userModel => userModel.Name == user.Name) ?? Add(user);
        }

        private ILiteQueryable<User> GetQueryable(ILiteRepository repository)
        {
            return repository
                .Query<User>(PostcrossingTrackerConstants.UserCollectionName)
                .Include(user => user.Country);
        }
    }
}
