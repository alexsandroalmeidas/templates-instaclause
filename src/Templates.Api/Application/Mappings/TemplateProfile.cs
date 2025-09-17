using AutoMapper;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;

namespace Templates.Api.Application.Mappings
{
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            CreateMap<Template, TemplateDto>();

            CreateMap<TemplateCreateDto, Template>();

            CreateMap<TemplateUpdateDto, Template>();
        }
    }
}
