using Profiles;

namespace Application.Filters;

public class PatientFilter
{
    public string? FullName { get; set; }

    public IEnumerable<PatientProfile> Filter(IEnumerable<PatientProfile> profiles)
    {
        return profiles
            .Where(p =>
                {
                    var fullName = $"{p.FirstName} {p.LastName} {p.MiddleName}";
                    return FullName is null || fullName.Contains(FullName, StringComparison.OrdinalIgnoreCase);
                })
            .OrderBy(p => $"{p.FirstName} {p.LastName} {p.MiddleName}");
    }
}
