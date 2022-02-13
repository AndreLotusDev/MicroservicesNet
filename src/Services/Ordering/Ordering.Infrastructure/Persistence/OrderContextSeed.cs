using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            var doesntHaveAnythingAlready = !orderContext.Orders.Any();
            if (doesntHaveAnythingAlready)
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "swn", 
                    FirstName = "Andre", 
                    LastName = "Soares", 
                    EmailAddress = "andrsoares953@yahoo.com", 
                    AddressLine = "Rua Alagoas", 
                    Country = "Brasil", 
                    TotalPrice = 1090 
                }
            };

        }
    }
}
