namespace Templates.Api.Application.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string Street { get; set; } = default!;

        public string HouseNumber { get; set; } = default!;

        public string City { get; set; } = default!;

        public string State { get; set; } = default!;

        public string Country { get; set; } = default!;

        public string ZipCode { get; set; } = default!;
    }
}
