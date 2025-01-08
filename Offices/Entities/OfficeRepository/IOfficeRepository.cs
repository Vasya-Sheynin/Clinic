using Offices;

namespace OfficeRepositories;

public interface IOfficeRepository
{
    Task<IEnumerable<Office>?> GetOffices();
    Task<Office?> GetOffice(Guid id);
    Task CreateOffice(Office office);
    Task UpdateOffice(Office office);
    Task DeleteOffice(Office office);
}
