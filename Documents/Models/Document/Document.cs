using System.ComponentModel.DataAnnotations;

namespace Documents;

public class Document
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ResultId { get; set; }

    public string? Name { get; set; }
}
