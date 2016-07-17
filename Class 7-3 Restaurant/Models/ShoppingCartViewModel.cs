using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Data;

namespace Class_7_3_Restaurant.Models
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<MenuItemWithQuantity> ItemsWithQuantity { get; set; }
    }
}