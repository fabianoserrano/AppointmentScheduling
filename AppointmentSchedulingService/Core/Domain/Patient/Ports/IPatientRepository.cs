namespace Domain.Ports
{
    public interface IPatientRepository
    {
        Task<int> Create(Domain.Entities.Patient patient);
        Task<Domain.Entities.Patient> Get(int Id);
        Task<int> Update(Domain.Entities.Patient patient);
        Task Delete(int Id);
        Task<Domain.Entities.Patient> FindByLogin(string email, string password);
    }
}
