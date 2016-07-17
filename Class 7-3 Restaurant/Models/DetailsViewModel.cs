using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Data;
namespace Class_7_3_Restaurant.Models
{
    public class DetailsViewModel
    {
        public IEnumerable<MenuItemWithQuantity> MenuItems { get; set; }
        public int OrderId { get; set; }
    }
}