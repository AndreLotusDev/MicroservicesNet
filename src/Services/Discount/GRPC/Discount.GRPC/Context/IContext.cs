using System.Data;
using System.Data.Common;

namespace Discount.GRPC.Context
{
    public interface IContext
    {
        public IDbConnection ContextDB { get; }
    }
}
