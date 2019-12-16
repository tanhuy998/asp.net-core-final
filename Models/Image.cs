using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Path { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
