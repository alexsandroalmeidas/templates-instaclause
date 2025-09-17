using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Data.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        public Address Address { get; set; } = new();

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
