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
    public class PostcardRepository(IRepositoryService repositoryService) : IPostcardRepository
    {
        private readonly IRepositoryService _repositoryService = repositoryService;

        public async Task<Postcard> Add(Postcard postcard)
        {
            if (postcard == null
                || string.IsNullOrWhiteSpace(postcard.PostcardId)
                || postcard.Country == null
                || postcard.Country.Id == 0)
            {
                return null;
            }

            using var repository = _repositoryService.GetRepository();

            await repository.InsertAsync(postcard, PostcrossingTrackerConstants.PostcardCollectionName).ConfigureAwait(false);

            return postcard;
        }

        public async Task<IEnumerable<Postcard>> All()
        {
            using var repository = _repositoryService.GetRepository();

            return await GetQueryable(repository)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<Postcard> Get(Expression<Func<Postcard, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return await GetQueryable(repository)
                .Where(predicate)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<Postcard> GetOrAdd(Postcard postcard)
        {
            return await Get(postcardModel => postcardModel.PostcardId == postcard.PostcardId).ConfigureAwait(false) ?? await Add(postcard).ConfigureAwait(false);
        }

        private static ILiteQueryableAsync<Postcard> GetQueryable(ILiteRepositoryAsync repository) => repository
                .Query<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName)
                .IncludeAll();
    }
}
