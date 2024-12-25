using ProfileRepositories.Pagination;
using Profiles;

namespace ProfileRepositories;

public interface IDoctorProfileRepo
{
    IEnumerable<DoctorProfile>? GetDoctorProfiles(DoctorFilter filterParams, PaginationParams paginationParams);
    Task<DoctorProfile?> GetDoctorProfileAsync(Guid id);
    Task CreateDoctorProfileAsync(DoctorProfile profile);
    Task UpdateDoctorProfileAsync(DoctorProfile newProfile);
    Task DeleteDoctorProfileAsync(DoctorProfile profile);
}
