using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }
        public string Slug { get; set; }

        public ICollection<Image> Images { get; set; }
        public Category Category { get; set; }
        public IEnumerable<OrderProduct> Orders { get; set; }
    }
}
