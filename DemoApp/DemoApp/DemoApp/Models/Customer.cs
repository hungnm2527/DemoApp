using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string Name { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string Address { get; set; }
        public List<Order> Orders { get; set; }
    }
}
