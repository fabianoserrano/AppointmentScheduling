namespace Domain.Ports
{
    public interface IDoctorRepository
    {
        Task<int> Create(Domain.Entities.Doctor doctor);
        Task<Domain.Entities.Doctor> Get(int Id);
        Task<List<Domain.Entities.Doctor>> Get();
        Task<int> Update(Domain.Entities.Doctor doctor);
        Task Delete(int Id);
        Task<Domain.Entities.Doctor> FindByLogin(string email, string password);
    }
}
