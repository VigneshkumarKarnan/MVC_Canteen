using System;
using System.ComponentModel.DataAnnotations;

namespace EX_2_MVC.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "Cash", "Card", "UPI"

        // Optional fields based on payment method
        public string? CardNumber { get; set; }
        public string? UPIId { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
