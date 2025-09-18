using Templates.Api.Application.Dtos;

namespace Templates.Api.Application.Services
{
    public interface ITemplatesService
    {
        Task<IEnumerable<TemplateDto>> GetTemplatesAsync(CancellationToken cancellationToken);
        Task<TemplateDto?> GetTemplateByIdAsync(int id, CancellationToken cancellationToken);
        Task<string?> GetTemplateByUserIdAsync(int id, int userId, CancellationToken cancellationToken);
        Task<string?> GetTemplateByUserIdHtmlAsync(int id, int userId, CancellationToken cancellationToken);
        Task<TemplateDto> CreateTemplateAsync(TemplateCreateDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateTemplateAsync(TemplateUpdateDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteTemplateAsync(int id, CancellationToken cancellationToken);
    }
}
