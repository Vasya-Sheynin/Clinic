using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using OfficeRepositories;
using Offices;

namespace Persistence
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public OfficeRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task CreateOffice(Office office)
        {
            using var connection = _sqlConnectionFactory.GetConnection();
            var sql = "INSERT INTO Offices (Id, PhotoId, Address, RegistryPhoneNumber, IsActive) VALUES (@Id, @PhotoId, @Address, @RegistryPhoneNumber, @IsActive)";
            await connection.ExecuteAsync(sql, office);
        }

        public async Task DeleteOffice(Office office)
        {
            using var connection = _sqlConnectionFactory.GetConnection();
            var sql = "DELETE FROM Offices WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { office.Id });
        }

        public async Task<Office?> GetOffice(Guid id)
        {
            using var connection = _sqlConnectionFactory.GetConnection();
            var sql = "SELECT Id, PhotoId, Address, RegistryPhoneNumber, IsActive FROM Offices WHERE Id = @id";
            var office = await connection.QueryFirstOrDefaultAsync<Office>(sql, new { Id = id });

            return office;
        }

        public async Task<IEnumerable<Office>?> GetOffices()
        {
            using var connection = _sqlConnectionFactory.GetConnection();
            var sql = "SELECT Id, PhotoId, Address, RegistryPhoneNumber, IsActive FROM Offices";
            var offices = await connection.QueryAsync<Office>(sql);

            return offices;
        }

        public async Task UpdateOffice(Office office)
        {
            using var connection = _sqlConnectionFactory.GetConnection();
            var sql = "UPDATE Offices SET PhotoId = @PhotoId, Address = @Address, RegistryPhoneNumber = @RegistryPhoneNumber, IsActive = @IsActive WHERE Id = @Id";
            await connection.ExecuteAsync(sql, office);
        }
    }
}
