namespace Profiles;

public class DoctorProfile
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Status { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CareerStartDate { get; set; }
}
