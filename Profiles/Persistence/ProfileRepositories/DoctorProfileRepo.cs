using Microsoft.EntityFrameworkCore;
using ProfileRepositories;
using Profiles;

namespace Persistence.ProfileRepositories;

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
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task DeleteDoctorProfileAsync(DoctorProfile profile)
    {
        _profilesDbContext.Remove(profile);
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task<DoctorProfile?> GetDoctorProfileAsync(Guid id)
    {
        var profile = await _profilesDbContext.DoctorProfiles.FirstOrDefaultAsync(p =>  p.Id == id);

        return profile;
    }

    public IEnumerable<DoctorProfile>? GetDoctorProfiles()
    {
        var profiles = _profilesDbContext.DoctorProfiles.Select(p => new DoctorProfile
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
        await _profilesDbContext.SaveChangesAsync();
    }
}
