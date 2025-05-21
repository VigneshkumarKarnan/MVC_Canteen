using System.ComponentModel.DataAnnotations;

namespace EX_2_MVC.ViewModels
{
    public class PaymentViewModel : IValidatableObject
    {
        public int OrderId { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string? CardNumber { get; set; }

        public string? UPIId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentMethod == "Card" && string.IsNullOrWhiteSpace(CardNumber))
            {
                yield return new ValidationResult("Card Number is required for Card payments.", new[] { "CardNumber" });
            }

            if (PaymentMethod == "UPI" && string.IsNullOrWhiteSpace(UPIId))
            {
                yield return new ValidationResult("UPI ID is required for UPI payments.", new[] { "UPIId" });
            }
        }
    }
}
