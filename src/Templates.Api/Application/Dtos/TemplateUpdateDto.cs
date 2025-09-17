using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class TemplateUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; } = default!;
    }
}
