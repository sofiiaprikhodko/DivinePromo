using System.ComponentModel.DataAnnotations;

namespace DivinePromo.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime SubscriptionDate { get; set; } = DateTime.Now;
    }
}