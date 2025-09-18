using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Application.Dtos
{
    public class TemplateUpdateDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; } = default!;

        [Required]
        public string Value { get; set; } = default!;
    }
}
