using Checkout.Infrastructure.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Checkout.Business.Contract
{
    public interface IBusinessServiceBase<TEntity> where TEntity : BaseEntity
    {
        TEntity GetById(int id);

        IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedBy = null, string includeProperties = null);

        TEntity Save(TEntity toSave);

        void Remove(int id);
    }
}
