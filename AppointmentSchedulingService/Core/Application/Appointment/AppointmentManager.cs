using Application.Appointment.Dtos;
using Application.Appointment.Ports;
using Application.Appointment.Requests;
using Application.Appointment.Responses;
using Domain.Appointment.Exceptions;
using Domain.Appointment.Ports;
using Domain.Ports;

namespace Application.Appointment
{
    public class AppointmentManager : IAppointmentManager
    {
        private IAppointmentRepository _appointmentRepository;
        private IDoctorRepository _doctorRepository;
        private IPatientRepository _patientRepository;

        public AppointmentManager(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest request)
        {
            try
            {
                var appointment = CreateAppointmentDto.MapToEntity(request.Data);

                var sameDateAppointments = await _appointmentRepository.GetAppointments(request.Data.DoctorId, request.Data.Date);

                if (sameDateAppointments != null && 
                    sameDateAppointments.Any(a => a.Date >= request.Data.Date.AddMinutes(-29) && a.Date <= request.Data.Date.AddMinutes(29)))
                {
                    return new CreateAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.APPOINTMENT_TIME_OVERLAPPING,
                        Message = "Overlapping appointment hours"
                    };
                }

                appointment.Doctor = await _doctorRepository.Get(request.Data.DoctorId);

                await appointment.Save(_appointmentRepository);
                request.Data.Id = appointment.Id;

                return new CreateAppointmentResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (MissingRequiredInformation)
            {
                return new CreateAppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (DoctorIsRequiredInformation)
            {
                return new CreateAppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION,
                    Message = "The doctor id provided was not found"
                };
            }
            catch (Exception)
            {
                return new CreateAppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }

        public async Task<AppointmentResponse> GetAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepository.Get(appointmentId);

            if (appointment == null) 
            {
                return new AppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_NOT_FOUND,
                    Message = "No Appointment record was found with the given Id"
                };
            }

            return new AppointmentResponse
            {
                Data = AppointmentDto.MapToDto(appointment),
                Success = true,
            };
        }

        public async Task<AvailableAppointmentsResponse> GetAvailableAppointments(int doctorId)
        {
            if (doctorId < 1)
            {
                return new AvailableAppointmentsResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION,
                    Message = "Invalid doctor id"
                };
            }

            var appointments = await _appointmentRepository.GetAvailableAppointments(doctorId);

            if (appointments == null)
            {
                return new AvailableAppointmentsResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_NOT_FOUND,
                    Message = "No Appointment record was found with the given Id"
                };
            }

            var data = new List<AvailableAppointmentDto>();
            appointments.ForEach(appointment => data.Add(AvailableAppointmentDto.MapToDto(appointment)));

            return new AvailableAppointmentsResponse
            {
                Data = data,
                Success = true,
            };
        }

        public async Task<AppointmentResponse> UpdateAppointment(UpdateAppointmentRequest request)
        {
            try
            {
                var appointment = AppointmentDto.MapToEntity(request.Data);

                appointment.Doctor = await _doctorRepository.Get(request.Data.DoctorId);

                await appointment.Save(_appointmentRepository);
                request.Data.Id = appointment.Id;

                return new AppointmentResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new AppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_UPDATE_FAILED,
                    Message = "There was an error when updating to DB"
                };
            }
        }

        public async Task<ScheduleAppointmentResponse> ScheduleAppointment(ScheduleAppointmentRequest request)
        {
            try
            {
                if (request.Data.Id < 1)
                {
                    return new ScheduleAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION,
                        Message = "Invalid appointment id"
                    };
                }

                if (request.Data.PatientId < 1)
                {
                    return new ScheduleAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION,
                        Message = "Invalid patient id"
                    };
                }

                var appointment = await _appointmentRepository.Get(request.Data.Id);

                if (appointment == null)
                {
                    return new ScheduleAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.APPOINTMENT_NOT_FOUND,
                        Message = "No Appointment record was found with the given Id"
                    };
                }

                if (appointment.Patient != null)
                {
                    return new ScheduleAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.APPOINTMENT_ALREADY_SCHEDULED,
                        Message = "Appointment already scheduled"
                    };
                }

                appointment.Patient = await _patientRepository.Get(request.Data.PatientId);

                if (appointment.Patient == null)
                {
                    return new ScheduleAppointmentResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.PATIENT_NOT_FOUND,
                        Message = "No Patient record was found with the given Id"
                    };
                }

                await appointment.Save(_appointmentRepository);
                request.Data.Id = appointment.Id;

                return new ScheduleAppointmentResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new ScheduleAppointmentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.COULD_NOT_STORE_DATA,
                    Message = "There was an error when updating to DB"
                };
            }
        }

        public async Task<AppointmentResponse> DeleteAppointment(int appointmentId)
        {
            try
            {
                await _appointmentRepository.Delete(appointmentId);

                return new AppointmentResponse
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new AppointmentResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.APPOINTMENT_DELETE_FAILED,
                    Message = "There was an error when deleting to DB"
                };
            }
        }
    }
}
