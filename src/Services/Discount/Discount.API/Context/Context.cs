using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Discount.API.Context
{
    public class Context : IContext
    {
        public IDbConnection ContextDB { get; }
        public Context(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionPostgres");
            ContextDB = new NpgsqlConnection(connectionString);
        }
    }
}
