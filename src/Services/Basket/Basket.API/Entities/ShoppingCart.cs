using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public decimal TotalPrice => Items.Sum(s => s.Price * s.Quantity);

        public ShoppingCart()
        {

        }

        public ShoppingCart(string username)
        {
            UserName = username;
        }
    }
}
