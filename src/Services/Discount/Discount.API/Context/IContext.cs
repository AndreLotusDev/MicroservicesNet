using System.Data;
using System.Data.Common;

namespace Discount.API.Context
{
    public interface IContext
    {
        public IDbConnection ContextDB { get; }
    }
}
