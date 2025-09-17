using System.ComponentModel.DataAnnotations;

namespace Templates.Api.Data.Entities
{
    public class Template : BaseEntity
    {
        [Required]
        public string Value { get; set; } = string.Empty;
    }
}
