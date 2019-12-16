using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PayerName { get; set; }
        public string PayerNumber { get; set; }
        public DateTime Time { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod Method { get; set; }
    }
}
