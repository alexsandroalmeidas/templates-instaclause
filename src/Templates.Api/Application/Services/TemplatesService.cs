using AutoMapper;
using Templates.Api.Application.Dtos;
using Templates.Api.Data.Entities;
using Templates.Api.Data.Repositories;

namespace Templates.Api.Application.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly IRepository<Template> _templatesRepository;
        private readonly IMapper _mapper;

        public TemplatesService(IRepository<Template> templatesRepository, IMapper mapper)
        {
            _templatesRepository = templatesRepository ?? throw new ArgumentNullException(nameof(templatesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TemplateDto>> GetTemplatesAsync(CancellationToken cancellationToken)
        {
            var templates = await _templatesRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TemplateDto>>(templates);
        }

        public async Task<TemplateDto?> GetTemplateByIdAsync(int id, CancellationToken cancellationToken)
        {
            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);

            return template == null
                    ? null
                    : _mapper.Map<TemplateDto>(template);
        }

        public async Task<TemplateDto?> GetTemplateByUserIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);

            return template == null
                    ? null
                    : _mapper.Map<TemplateDto>(template);
        }

        public async Task<TemplateDto> CreateTemplateAsync(TemplateCreateDto dto, CancellationToken cancellationToken)
        {
            var template = _mapper.Map<Template>(dto);

            await _templatesRepository.AddAsync(template, cancellationToken);

            return _mapper.Map<TemplateDto>(template);
        }

        public async Task<bool> UpdateTemplateAsync(TemplateUpdateDto dto, CancellationToken cancellationToken)
        {
            var template = await _templatesRepository.GetByIdAsync(dto.Id, cancellationToken);

            if (template == null) return false;

            _mapper.Map(dto, template);

            await _templatesRepository.UpdateAsync(template, cancellationToken);

            return true;
        }

        public async Task<bool> DeleteTemplateAsync(int id, CancellationToken cancellationToken)
        {
            var template = await _templatesRepository.GetByIdAsync(id, cancellationToken);

            if (template == null) return false;

            await _templatesRepository.DeleteAsync(template, cancellationToken);

            return true;
        }
    }
}
