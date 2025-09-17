using AutoMapper;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;

namespace Templates.Api.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UserCreateDto, User>();

            CreateMap<UserUpdateDto, User>();
        }
    }
}
