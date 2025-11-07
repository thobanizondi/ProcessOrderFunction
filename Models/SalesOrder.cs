using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessOrderFunction.Models
{

    public class SalesOrder
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}