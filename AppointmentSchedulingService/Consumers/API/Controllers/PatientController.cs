using Application;
using Application.Patient.Dtos;
using Application.Patient.Ports;
using Application.Patient.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientManager _patientManager;

        public PatientController(ILogger<PatientController> logger, IPatientManager patientManager)
        {
            _logger = logger;
            _patientManager = patientManager;
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Post(PatientDto patient)
        {
            var request = new CreatePatientRequest
            {
                Data = patient
            };

            var res = await _patientManager.CreatePatient(request);

            if (res.Success) return Created("", res.Data);

            if (res.ErrorCode == ErrorCodes.PATIENT_NOT_FOUND)
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

        [Authorize("Bearer", Roles = "Patient")]
        [HttpGet]
        public async Task<ActionResult<PatientDto>> Get(int patientId)
        {
            var res = await _patientManager.GetPatient(patientId);
            
            if (res.Success) return Created("", res.Data);
            
            return NotFound(res);
        }

        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult<PatientDto>> Put(PatientDto patient)
        {
            var request = new UpdatePatientRequest
            {
                Data = patient
            };

            var res = await _patientManager.UpdatePatient(request);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            if (res.Success) return Ok(res.Data);

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpDelete]
        public async Task<ActionResult<PatientDto>> Delete(int patientId)
        {
            var res = await _patientManager.DeletePatient(patientId);

            if(res.Success) return Ok(res.Data);

            return NotFound(res);
        }
    }
}
