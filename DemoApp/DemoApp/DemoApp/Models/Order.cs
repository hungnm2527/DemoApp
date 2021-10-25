using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid ProductID { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string OrderName { get; set; }
        public int Amount { get; set; }
        [Column(TypeName = "Datetime")]
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
