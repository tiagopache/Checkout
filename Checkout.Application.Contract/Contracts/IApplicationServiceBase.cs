using Checkout.Application.Contract.ViewModels;
using System.Collections.Generic;

namespace Checkout.Application.Contract.Contracts
{
    public interface IApplicationServiceBase<TContract> where TContract : BaseViewModel
    {
        //IList<TContract> Get(Expression<Func<TContract, bool>> filter = null, Func<IQueryable<TContract>, IOrderedQueryable<TContract>> orderedBy = null, string includeProperties = null);
        IList<TContract> Get(string includeProperties = null);
    }
}
