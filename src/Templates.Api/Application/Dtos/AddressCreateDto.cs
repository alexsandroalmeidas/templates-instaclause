using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class AddressCreateDto
    {
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
