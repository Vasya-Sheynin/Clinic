using Microsoft.EntityFrameworkCore;
using ProfileRepositories;
using ProfileRepositories.Pagination;
using Profiles;

namespace Persistence.ProfileRepositories;

public class PatientProfileRepo : IPatientProfileRepo
{
    private readonly ProfilesDbContext _profilesDbContext;

    public PatientProfileRepo(ProfilesDbContext profilesDbContext)
    {
        _profilesDbContext = profilesDbContext;
    }

    public async Task CreatePatientProfileAsync(PatientProfile profile)
    {
        await _profilesDbContext.AddAsync(profile);
    }

    public async Task DeletePatientProfileAsync(PatientProfile profile)
    {
        _profilesDbContext.Remove(profile);
    }

    public async Task<PatientProfile?> GetPatientProfileAsync(Guid id)
    {
        var profile = await _profilesDbContext.PatientProfiles.FirstOrDefaultAsync(p => p.Id == id);
        
        return profile;
    }

    public IEnumerable<PatientProfile>? GetPatientProfiles(PatientFilter filterParams, PaginationParams paginationParams)
    {
        var query = _profilesDbContext.PatientProfiles.AsQueryable();

        if (!string.IsNullOrEmpty(filterParams.FullName))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName + " " + p.LastName + " " + (p.MiddleName != null ? p.MiddleName : string.Empty), "%" + filterParams.FullName + "%")
            );
        }

        var profiles = query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ThenBy(p => p.MiddleName)
            .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(p => new PatientProfile
            {
                Id = p.Id,
                AccountId = p.AccountId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                IsLinkedToAccount = p.IsLinkedToAccount,
                DateOfBirth = p.DateOfBirth,
            })
            .AsEnumerable();

        return profiles;
    }

    public async Task UpdatePatientProfileAsync(PatientProfile newProfile)
    {
        _profilesDbContext.Update(newProfile);
    }
}
