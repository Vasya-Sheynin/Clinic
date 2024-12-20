using Profiles;

namespace Application.Filters;

public class DoctorFilter
{
    public string? FullName { get; set; }
    public Guid? SpecializationId { get; set; }
    public Guid? OfficeId { get; set; }

    public IEnumerable<DoctorProfile> Filter(IEnumerable<DoctorProfile> profiles)
    {
        return profiles
            .Where(p =>
            {
                var fullName = $"{p.FirstName} {p.LastName} {p.MiddleName}";

                return (FullName is null || fullName.Contains(FullName, StringComparison.OrdinalIgnoreCase)) &&
                    (SpecializationId is null || p.SpecializationId == SpecializationId) &&
                    (OfficeId is null || p.OfficeId == OfficeId);
            })
            .OrderBy(p => $"{p.FirstName} {p.LastName} {p.MiddleName}");
    }
}
