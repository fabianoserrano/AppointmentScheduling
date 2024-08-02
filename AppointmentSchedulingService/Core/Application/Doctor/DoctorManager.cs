using Application.Doctor.Dtos;
using Application.Doctor.Ports;
using Application.Doctor.Requests;
using Application.Doctor.Responses;
using Domain.Ports;
using Domain.User.Exceptions;

namespace Application.Doctor
{
    public class DoctorManager : IDoctorManager
    {
        private IDoctorRepository _doctorRepository;
        public DoctorManager(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        public async Task<DoctorResponse> CreateDoctor(CreateDoctorRequest request)
        {
            try
            {
                var doctor = DoctorDto.MapToEntity(request.Data);

                await doctor.Save(_doctorRepository);

                request.Data.Id = doctor.Id;

                return new DoctorResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (MissingRequiredInformation e)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
        public async Task<DoctorResponse> GetDoctor(int doctorId)
        {
            var doctor = await _doctorRepository.Get(doctorId);

            if (doctor == null)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DOCTOR_NOT_FOUND,
                    Message = "No Doctor record was found with the given Id"
                };
            }

            return new DoctorResponse
            {
                Data = DoctorDto.MapToDto(doctor),
                Success = true,
            };
        }
        public async Task<DoctorsNoPasswordResponse> GetDoctors()
        {
            var doctors = await _doctorRepository.Get();

            if (doctors == null)
            {
                return new DoctorsNoPasswordResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DOCTOR_NOT_FOUND,
                    Message = "No Doctor record was found with the given Id"
                };
            }

            var data = new List<DoctorNoPasswordDto>();
            doctors.ForEach(doctor => data.Add(DoctorNoPasswordDto.MapToDto(doctor)));

            return new DoctorsNoPasswordResponse
            {
                Data = data,
                Success = true,
            };
        }
        public async Task<DoctorResponse> UpdateDoctor(UpdateDoctorRequest request)
        {
            try
            {
                var doctor = DoctorDto.MapToEntity(request.Data);

                await doctor.Save(_doctorRepository);
                request.Data.Id = doctor.Id;
                
                return new DoctorResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (MissingRequiredInformation e)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (InvalidEmailException e)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "The given email is not valid"
                };
            }
            catch (Exception)
            {
                return new DoctorResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DOCTOR_UPDATE_FAILED,
                    Message = "There was an error when updating to DB"
                };
            }
        }
        public async Task<DoctorResponse> DeleteDoctor(int doctorId)
        {
            try
            {
                await _doctorRepository.Delete(doctorId);

                return new DoctorResponse
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new DoctorResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DOCTOR_DELETE_FAILED,
                    Message = "There was an error when deleting to DB"
                };
            }
        }
    }
}
