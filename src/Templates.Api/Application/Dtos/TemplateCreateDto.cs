using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class TemplateCreateDto
    {
        [Required]
        public string Value { get; set; } = default!;
    }
}
