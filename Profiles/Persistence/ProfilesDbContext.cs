using Microsoft.EntityFrameworkCore;
using Profiles;

namespace Persistence;

public class ProfilesDbContext : DbContext
{
    public DbSet<DoctorProfile> DoctorProfiles { get; set; }
    public DbSet<PatientProfile> PatientProfiles { get; set; }
    public DbSet<ReceptionistProfile> ReceptionistProfiles { get; set; }

    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options)
    {
        
    }
}
