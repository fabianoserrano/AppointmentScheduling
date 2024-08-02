using Application;
using Application.Appointment.Dtos;
using Application.Appointment.Ports;
using Application.Appointment.Requests;
using Application.Email.Ports;
using Application.Email.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentManager _appointmentManager;
        private readonly IEmailManager _emailManager;

        public AppointmentController(
            ILogger<AppointmentController> logger,
            IAppointmentManager appointmentManager,
            IEmailManager emailManager)
        {
            _logger = logger;
            _appointmentManager = appointmentManager;
            _emailManager = emailManager;
        }

        //[Authorize("Bearer")]
        [HttpPost]
        public async Task<ActionResult<CreateAppointmentDto>> Post(CreateAppointmentDto appointment)
        {
            var request = new CreateAppointmentRequest
            {
                Data = appointment
            };

            var res = await _appointmentManager.CreateAppointment(request);

            if(res.Success) return Created("", res.Data);

            if (res.ErrorCode == ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.APPOINTMENT_NOT_FOUND)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.APPOINTMENT_TIME_OVERLAPPING)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpGet]
        public async Task<ActionResult<AppointmentDto>> Get(int appointmentId)
        {
            var res = await _appointmentManager.GetAppointment(appointmentId);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        //[Authorize("Bearer", Roles = Roles.Patient)]
        [HttpGet]
        [Route("GetAvailableAppointments")]
        public async Task<ActionResult<AvailableAppointmentDto>> GetAvailableAppointments(int doctorId)
        {
            var res = await _appointmentManager.GetAvailableAppointments(doctorId);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.APPOINTMENT_NOT_FOUND) return BadRequest(res);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult<AppointmentDto>> Put(AppointmentDto appointment)
        {
            var request = new UpdateAppointmentRequest
            {
                Data = appointment
            };

            var res = await _appointmentManager.UpdateAppointment(request);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            if (res.Success) return Ok(res.Data);

            _logger.LogError("Response with unknown ErrorCode Returned", res);

            return BadRequest(500);
        }

        //[Authorize("Bearer", Roles = Roles.Patient)]
        [HttpPut]
        [Route("ScheduleAppointment")]
        public async Task<ActionResult<ScheduleAppointmentDto>> ScheduleAppointment(ScheduleAppointmentDto appointment)
        {
            var request = new ScheduleAppointmentRequest
            {
                Data = appointment
            };

            var res = await _appointmentManager.ScheduleAppointment(request);

            if (res.ErrorCode == ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.APPOINTMENT_NOT_FOUND) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.APPOINTMENT_ALREADY_SCHEDULED) return BadRequest(res); 

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            if (res.Success)
            {
                var emailRequest = new SendEmailRequest
                {
                    Data = new Application.Email.Dtos.EmailDto() { AppointmentId = res.Data.Id },
                };

                await _emailManager.SendEmail(emailRequest);

                return Ok(res.Data);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);

            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpDelete]
        public async Task<ActionResult<AppointmentDto>> Delete(int appointmentId)
        {
            var res = await _appointmentManager.DeleteAppointment(appointmentId);

            if (res.Success) return Ok(res.Data);

            return NotFound(res);
        }
    }
}
