using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2AdoNetDemo
{
    public class Product
    {
        public int ProductId { get; set; }      // maps to dbo.Products.ProductId
        public required string ProductName { get; set; } // maps to dbo.Products.ProductName
        public int CategoryId { get; set; }     // maps to dbo.Products.CategoryId
        public decimal UnitPrice { get; set; }  // maps to dbo.Products.UnitPrice
        public bool IsActive { get; set; }      // maps to dbo.Products.IsActive
    }
}
