using ProfileRepositories.Pagination;
using Profiles;

namespace ProfileRepositories;

public interface IPatientProfileRepo
{
    IEnumerable<PatientProfile>? GetPatientProfiles(PatientFilter filterParams, PaginationParams paginationParams);
    Task<PatientProfile?> GetPatientProfileAsync(Guid id);
    Task CreatePatientProfileAsync(PatientProfile profile);
    Task UpdatePatientProfileAsync(PatientProfile newProfile);
    Task DeletePatientProfileAsync(PatientProfile profile);
}
