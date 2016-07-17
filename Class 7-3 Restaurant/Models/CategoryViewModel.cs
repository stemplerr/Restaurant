using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Data;
using System.Web;

namespace Class_7_3_Restaurant.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}