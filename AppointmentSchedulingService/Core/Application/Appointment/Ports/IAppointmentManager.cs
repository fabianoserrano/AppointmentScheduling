using Application.Appointment.Requests;
using Application.Appointment.Responses;

namespace Application.Appointment.Ports
{
    public interface IAppointmentManager
    {
        Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest request);
        Task<AppointmentResponse> GetAppointment(int appointmentId);
        Task<AvailableAppointmentsResponse> GetAvailableAppointments(int doctorId);
        Task<AppointmentResponse> UpdateAppointment(UpdateAppointmentRequest request);
        Task<ScheduleAppointmentResponse> ScheduleAppointment(ScheduleAppointmentRequest request);
        Task<AppointmentResponse> DeleteAppointment(int appointmentId);
    }
}
