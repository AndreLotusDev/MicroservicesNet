using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var clientConnection = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MongoShopping"));
            var database = clientConnection.GetDatabase(configuration.GetValue<string>("ConnectionStrings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("ConnectionStrings:CollectionName"));
        }

        public IMongoCollection<Product> Products { get; }
    }
}
