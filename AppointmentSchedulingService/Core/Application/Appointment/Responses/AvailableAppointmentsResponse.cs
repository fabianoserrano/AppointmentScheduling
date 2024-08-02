using Application.Appointment.Dtos;

namespace Application.Appointment.Responses
{
    public class AvailableAppointmentsResponse : Response
    {
        public List<AvailableAppointmentDto> Data;
    }
}
