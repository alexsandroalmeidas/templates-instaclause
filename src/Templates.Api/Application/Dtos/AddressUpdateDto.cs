using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class AddressUpdateDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; } = default!;

        [MaxLength(200)]
        public string Street { get; set; } = default!;

        [MaxLength(20)]
        public string HouseNumber { get; set; } = default!;

        [MaxLength(100)]
        public string City { get; set; } = default!;

        [MaxLength(100)]
        public string State { get; set; } = default!;

        [MaxLength(100)]
        public string Country { get; set; } = default!;

        [MaxLength(20)]
        public string ZipCode { get; set; } = default!;
    }
}
