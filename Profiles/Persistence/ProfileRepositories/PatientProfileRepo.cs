using Microsoft.EntityFrameworkCore;
using ProfileRepositories;
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
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task DeletePatientProfileAsync(PatientProfile profile)
    {
        _profilesDbContext.Remove(profile);
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task<PatientProfile?> GetPatientProfileAsync(Guid id)
    {
        var profile = await _profilesDbContext.PatientProfiles.FirstOrDefaultAsync(p => p.Id == id);
        
        return profile;
    }

    public IEnumerable<PatientProfile>? GetPatientProfiles()
    {
        var profiles = _profilesDbContext.PatientProfiles.Select(p => new PatientProfile
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
        await _profilesDbContext.SaveChangesAsync();
    }
}
