using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EX_2_MVC.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        //[Range(0.01, 1000)]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; } // Optional: Snacks, Meals, Drinks, etc.
    }
}
