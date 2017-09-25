using System.Collections.Generic;

using Data.Core.Domain.Data;
using Data.Core.Domain.Model.Entities;

namespace Data.Core.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        #region Public Methods

        public static IFinder<TEntity> GetFinder<TEntity>(this IEnumerable<TEntity> source) where TEntity : IEntity
        {
            return new Finder<TEntity>(source);
        }

        #endregion
    }
}