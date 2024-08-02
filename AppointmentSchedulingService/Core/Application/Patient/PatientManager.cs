using Application.Patient.Dtos;
using Application.Patient.Ports;
using Application.Patient.Requests;
using Application.Patient.Responses;
using Domain.Ports;
using Domain.User.Exceptions;

namespace Application.Patient
{
    public class PatientManager : IPatientManager
    {
        private IPatientRepository _patientRepository;
        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<PatientResponse> CreatePatient(CreatePatientRequest request)
        {
            try
            {
                var patient = PatientDto.MapToEntity(request.Data);

                await patient.Save(_patientRepository);

                request.Data.Id = patient.Id;

                return new PatientResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (MissingRequiredInformation e)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
        public async Task<PatientResponse> GetPatient(int patientId)
        {
            var patient = await _patientRepository.Get(patientId);

            if (patient == null)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PATIENT_NOT_FOUND,
                    Message = "No Patient record was found with the given Id"
                };
            }

            return new PatientResponse
            {
                Data = PatientDto.MapToDto(patient),
                Success = true,
            };
        }
        public async Task<PatientResponse> UpdatePatient(UpdatePatientRequest request)
        {
            try
            {
                var patient = PatientDto.MapToEntity(request.Data);

                await patient.Save(_patientRepository);
                request.Data.Id = patient.Id;
                
                return new PatientResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (MissingRequiredInformation e)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new PatientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PATIENT_UPDATE_FAILED,
                    Message = "There was an error when updating to DB"
                };
            }
        }
        public async Task<PatientResponse> DeletePatient(int patientId)
        {
            try
            {
                await _patientRepository.Delete(patientId);

                return new PatientResponse
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new PatientResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PATIENT_DELETE_FAILED,
                    Message = "There was an error when deleting to DB"
                };
            }
        }
    }
}
