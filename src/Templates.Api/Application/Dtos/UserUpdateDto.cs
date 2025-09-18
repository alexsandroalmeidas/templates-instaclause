using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class UserUpdateDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; } = default!;

        [Required, StringLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = default!;

        [Required, StringLength(200), EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public AddressCreateDto Address { get; set; } = default!;
    }
}
