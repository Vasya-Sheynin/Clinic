using Microsoft.EntityFrameworkCore;
using ProfileRepositories;
using Profiles;

namespace Persistence.ProfileRepositories;

public class ReceptionistProfileRepo : IReceptionistProfileRepo
{
    private readonly ProfilesDbContext _profilesDbContext;

    public ReceptionistProfileRepo(ProfilesDbContext profilesDbContext)
    {
         _profilesDbContext = profilesDbContext;
    }

    public async Task CreateReceptionistProfileAsync(ReceptionistProfile profile)
    {
        await _profilesDbContext.AddAsync(profile);
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task DeleteReceptionistProfileAsync(ReceptionistProfile profile)
    {
        _profilesDbContext.Remove(profile);
        await _profilesDbContext.SaveChangesAsync();
    }

    public async Task<ReceptionistProfile?> GetReceptionistProfileAsync(Guid id)
    {
        var profile = await _profilesDbContext.ReceptionistProfiles.FirstOrDefaultAsync(p => p.Id == id);

        return profile;
    }

    public IEnumerable<ReceptionistProfile>? GetReceptionistProfiles()
    {
        var profiles = _profilesDbContext.ReceptionistProfiles.Select(p => new ReceptionistProfile
        {
            Id = p.Id,
            AccountId = p.AccountId,
            OfficeId = p.OfficeId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            MiddleName = p.MiddleName,
        }
            ).AsEnumerable();

        return profiles;
    }

    public async Task UpdateReceptionistProfileAsync(ReceptionistProfile newProfile)
    {
        _profilesDbContext.Update(newProfile);
        await _profilesDbContext.SaveChangesAsync();
    }
}
