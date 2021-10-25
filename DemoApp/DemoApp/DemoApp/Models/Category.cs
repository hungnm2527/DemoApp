using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryID { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string Name { get; set; }
        [Column(TypeName ="nvarchar(MAX)")]
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
