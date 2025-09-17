using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Data.Entities
{
    public class Address
    {
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(20)]
        public string HouseNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
