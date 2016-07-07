using Checkout.Infrastructure.Data;
using Checkout.Infrastructure.Data.Base;
using Checkout.Infrastructure.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Checkout.Business.Service
{
    public abstract class ServiceBase<TEntity> where TEntity : BaseEntity
    {
        [Dependency]
        protected IUnitOfWork<TEntity> UnitOfWork { get; set; }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderedBy = null, string includeProperties = null)
        {
            return this.UnitOfWork.Repository.Get(filter, orderedBy, includeProperties).ToList();
        }

        internal TEntity save(TEntity toSave, TEntity found)
        {
            if (found != null)
            {
                found = toSave.Copy<TEntity>();

                found.UpdatedOn = DateTime.Now;

                this.UnitOfWork.Repository.Update(found);

                toSave = found.Copy<TEntity>();
            }
            else
                this.UnitOfWork.Repository.Insert(toSave);

            this.UnitOfWork.Save();

            return toSave;
        }
    }

    public abstract class ServiceIdBase<TEntity> : ServiceBase<TEntity> where TEntity : BaseIdEntity
    {
        public virtual TEntity GetById(int id)
        {
            return this.UnitOfWork.Repository.GetById(id);
        }

        public virtual void Remove(int id)
        {
            this.UnitOfWork.Repository.Delete(id);

            this.UnitOfWork.Save();
        }

        public virtual TEntity Save(TEntity toSave)
        {
            var found = this.UnitOfWork.Repository.GetById(toSave.Id);

            return this.save(toSave, found);
        }
    }

    public abstract class ServiceGuidBase<TEntity> : ServiceBase<TEntity> where TEntity : BaseGuidEntity
    {
        public virtual TEntity GetById(Guid id)
        {
            return this.UnitOfWork.Repository.GetById(id);
        }

        public virtual void Remove(Guid id)
        {
            this.UnitOfWork.Repository.Delete(id);

            this.UnitOfWork.Save();
        }

        public virtual TEntity Save(TEntity toSave)
        {
            var found = this.UnitOfWork.Repository.GetById(toSave.Id);

            return this.save(toSave, found);
        }
    }
}
