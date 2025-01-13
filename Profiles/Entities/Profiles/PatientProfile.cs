using System.ComponentModel.DataAnnotations;

namespace Profiles;

public class PatientProfile
{
    [Key] public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool? IsLinkedToAccount { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
