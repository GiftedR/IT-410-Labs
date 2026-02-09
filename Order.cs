using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2AdoNetDemo
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = "";

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
