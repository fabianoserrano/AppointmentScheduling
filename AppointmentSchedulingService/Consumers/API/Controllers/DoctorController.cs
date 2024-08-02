using Application;
using Application.Doctor.Dtos;
using Application.Doctor.Ports;
using Application.Doctor.Requests;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorManager _doctorManager;

        public DoctorController(ILogger<DoctorController> logger, IDoctorManager doctorManager)
        {
            _logger = logger;
            _doctorManager = doctorManager;
        }

        [HttpPost]
        public async Task<ActionResult<DoctorDto>> Post(DoctorDto doctor)
        {
            var request = new CreateDoctorRequest
            {
                Data = doctor
            };

            var res = await _doctorManager.CreateDoctor(request);

            if (res.Success) return Created("", res.Data);

            if (res.ErrorCode == ErrorCodes.DOCTOR_NOT_FOUND)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.INVALID_EMAIL)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpGet]
        public async Task<ActionResult<DoctorDto>> Get(int doctorId)
        {
            var res = await _doctorManager.GetDoctor(doctorId);
            
            if (res.Success) return Created("", res.Data);
            
            return NotFound(res);
        }

        [Authorize("Bearer", Roles = Roles.Patient)]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<DoctorNoPasswordDto>>> Get()
        {
            var res = await _doctorManager.GetDoctors();

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult<DoctorDto>> Put(DoctorDto doctor)
        {
            var request = new UpdateDoctorRequest
            {
                Data = doctor
            };

            var res = await _doctorManager.UpdateDoctor(request);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            if (res.Success) return Ok(res.Data);

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpDelete]
        public async Task<ActionResult<DoctorDto>> Delete(int doctorId)
        {
            var res = await _doctorManager.DeleteDoctor(doctorId);

            if(res.Success) return Ok(res.Data);

            return NotFound(res);
        }
    }
}
