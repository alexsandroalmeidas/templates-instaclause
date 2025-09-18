using AutoMapper;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;

namespace Templates.Api.Application.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>();

            CreateMap<AddressCreateDto, Address>();

            CreateMap<AddressUpdateDto, Address>();
        }
    }
}
