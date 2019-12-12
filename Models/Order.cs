using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime Time { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public string ShipAdress { get; set; }
        public string ContactNumber { get; set; }

        public IEnumerable<OrderProduct> Products { get; set; }
        public Payment Payment { get; set; }
    }
}
