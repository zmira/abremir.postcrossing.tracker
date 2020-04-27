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
    public class PostcardRepository : IPostcardRepository
    {
        private readonly IRepositoryService _repositoryService;

        public PostcardRepository(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public Postcard Add(Postcard postcard)
        {
            if (postcard == null
                || string.IsNullOrWhiteSpace(postcard.PostcardId)
                || postcard.Country == null
                || postcard.Country.Id == 0)
            {
                return null;
            }

            postcard.TrimAllStrings();

            using var repository = _repositoryService.GetRepository();

            repository.Insert(postcard, PostcrossingTrackerConstants.PostcardCollectionName);

            return postcard;
        }

        public IEnumerable<Postcard> All()
        {
            using var repository = _repositoryService.GetRepository();

            return GetQueryable(repository)
                .ToList();
        }

        public Postcard Get(Expression<Func<Postcard, bool>> predicate)
        {
            using var repository = _repositoryService.GetRepository();

            return GetQueryable(repository)
                .Where(predicate)
                .FirstOrDefault();
        }

        public Postcard GetOrAdd(Postcard postcard)
        {
            return Get(postcardModel => postcardModel.PostcardId == postcard.PostcardId) ?? Add(postcard);
        }

        private ILiteQueryable<Postcard> GetQueryable(ILiteRepository repository)
        {
            return repository
                .Query<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName)
                .IncludeAll();
        }
    }
}
