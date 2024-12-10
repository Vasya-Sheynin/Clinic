using Profiles;

namespace ProfileRepositories;

public interface IReceptionistProfileRepo
{
    IEnumerable<ReceptionistProfile>? GetReceptionistProfiles();
    Task<ReceptionistProfile?> GetReceptionistProfileAsync(Guid id);
    Task CreateReceptionistProfileAsync(ReceptionistProfile profile);
    Task UpdateReceptionistProfileAsync(ReceptionistProfile newProfile);
    Task DeleteReceptionistProfileAsync(ReceptionistProfile profile);
}
