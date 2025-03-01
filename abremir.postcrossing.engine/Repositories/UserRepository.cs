using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Repositories
{
    public class UserRepository(IRepositoryService repositoryService) : IUserRepository
    {
        private readonly IRepositoryService _repositoryService = repositoryService;

        public async Task<User> Add(User user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.Name)
                || user.Country == null
                || user.Country.Id == 0)
            {
                return null;
            }

            using var repository = _repositoryService.GetRepository();

            await repository.InsertAsync(user, PostcrossingTrackerConstants.UserCollectionName).ConfigureAwait(false);

            return user;
        }

        public async Task<IEnumerable<User>> All()
        {
            using var repository = _repositoryService.GetRepository();

            return await GetQueryable(repository).ToListAsync().ConfigureAwait(false);
        }

        public async Task<User> Get(Expression<Func<User, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return await GetQueryable(repository)
                .Where(predicate)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<User> GetOrAdd(User user)
        {
            return await Get(userModel => userModel.Name == user.Name).ConfigureAwait(false) ?? await Add(user).ConfigureAwait(false);
        }

        private static ILiteQueryableAsync<User> GetQueryable(ILiteRepositoryAsync repository) => repository
                .Query<User>(PostcrossingTrackerConstants.UserCollectionName)
                .IncludeAll();
    }
}
