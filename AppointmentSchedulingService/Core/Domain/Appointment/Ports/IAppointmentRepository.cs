namespace Domain.Appointment.Ports
{
    public interface IAppointmentRepository
    {
        Task<Entities.Appointment> Get(int id);
        Task<List<Domain.Entities.Appointment>> GetAppointments(int doctorId, DateTime date);
        Task<List<Domain.Entities.Appointment>> GetAvailableAppointments(int doctorId);
        Task<int> Create(Entities.Appointment appointment);
        Task<int> Update(Entities.Appointment appointment);
        Task Delete(int id);
    }
}
