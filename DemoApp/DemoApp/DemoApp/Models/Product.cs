using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        public Guid CategoryID { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
        public List<Order> Orders { get; set; }
        public Category Category { get; set; }

    }
}
