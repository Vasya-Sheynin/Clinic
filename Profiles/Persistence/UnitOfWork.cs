using ProfileRepositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfilesDbContext _profilesDbContext;
    public IDoctorProfileRepo DoctorProfileRepo { get; private set; }

    public IPatientProfileRepo PatientProfileRepo { get; private set; }

    public IReceptionistProfileRepo ReceptionistProfileRepo { get; private set; }


    public UnitOfWork(
        ProfilesDbContext dbContext, 
        IDoctorProfileRepo doctorProfileRepo, 
        IPatientProfileRepo patientProfileRepo, 
        IReceptionistProfileRepo receptionistProfileRepo)
    {
        _profilesDbContext = dbContext;
        DoctorProfileRepo = doctorProfileRepo;
        PatientProfileRepo = patientProfileRepo;
        ReceptionistProfileRepo = receptionistProfileRepo;
    }
   
    public void Dispose()
    {
        _profilesDbContext.Dispose();
    }

    public async Task SaveAsync()
    {
        await _profilesDbContext.SaveChangesAsync();
    }
}
