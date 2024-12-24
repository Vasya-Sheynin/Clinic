using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Receptionist;

public record ReceptionistDto
{
    [Required] public Guid Id { get; set; }
    [Required] public Guid AccountId { get; set; }
    [Required] public Guid OfficeId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
}
