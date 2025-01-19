using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Patient;

public record UpdatePatientDto
{
    [Required] public Guid AccountId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool? IsLinkedToAccount { get; set; }
    [Required] public DateTime DateOfBirth { get; set; }
}
