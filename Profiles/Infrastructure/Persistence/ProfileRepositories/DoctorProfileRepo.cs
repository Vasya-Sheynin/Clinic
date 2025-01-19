using Microsoft.EntityFrameworkCore;
using ProfileRepositories;
using ProfileRepositories.Pagination;
using Profiles;

namespace Infrastructure.Persistence.ProfileRepositories;

public class DoctorProfileRepo : IDoctorProfileRepo
{
    private readonly ProfilesDbContext _profilesDbContext;

    public DoctorProfileRepo(ProfilesDbContext profilesDbContext)
    {
        _profilesDbContext = profilesDbContext;
    }

    public async Task CreateDoctorProfileAsync(DoctorProfile profile)
    {
        await _profilesDbContext.AddAsync(profile);
    }

    public async Task DeleteDoctorProfileAsync(DoctorProfile profile)
    {
        _profilesDbContext.Remove(profile);
    }

    public async Task<DoctorProfile?> GetDoctorProfileAsync(Guid id)
    {
        var profile = await _profilesDbContext.DoctorProfiles.FirstOrDefaultAsync(p => p.Id == id);

        return profile;
    }

    public IEnumerable<DoctorProfile>? GetDoctorProfiles(DoctorFilter filterParams, PaginationParams paginationParams)
    {
        var query = _profilesDbContext.DoctorProfiles.AsQueryable();

        if (!string.IsNullOrEmpty(filterParams.FullName))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName + " " + p.LastName + " " + (p.MiddleName != null ? p.MiddleName : string.Empty), "%" + filterParams.FullName + "%")
            );
        }

        if (filterParams.SpecializationId != null)
        {
            query = query.Where(p => p.SpecializationId == filterParams.SpecializationId);
        }

        if (filterParams.OfficeId != null)
        {
            query = query.Where(p => p.OfficeId == filterParams.OfficeId);
        }

        var profiles = query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ThenBy(p => p.MiddleName)
            .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(p => new DoctorProfile
            {
                Id = p.Id,
                AccountId = p.AccountId,
                SpecializationId = p.SpecializationId,
                OfficeId = p.OfficeId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                Status = p.Status,
                DateOfBirth = p.DateOfBirth,
                CareerStartDate = p.CareerStartDate,
            })
            .AsEnumerable();

        return profiles;
    }


    public async Task UpdateDoctorProfileAsync(DoctorProfile newProfile)
    {
        _profilesDbContext.Update(newProfile);
    }
}
