namespace ProfileRepositories;

public interface IUnitOfWork : IDisposable
{
    IDoctorProfileRepo DoctorProfileRepo { get; }
    IPatientProfileRepo PatientProfileRepo { get; }
    IReceptionistProfileRepo ReceptionistProfileRepo { get; }

    public Task SaveAsync(); 
}
