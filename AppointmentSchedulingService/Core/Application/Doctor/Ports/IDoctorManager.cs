using Application.Doctor.Requests;
using Application.Doctor.Responses;

namespace Application.Doctor.Ports
{
    public interface IDoctorManager
    {
        Task<DoctorResponse> CreateDoctor(CreateDoctorRequest request);
        Task<DoctorResponse> GetDoctor(int doctorId);
        Task<DoctorsNoPasswordResponse> GetDoctors();
        Task<DoctorResponse> UpdateDoctor(UpdateDoctorRequest request);
        Task<DoctorResponse> DeleteDoctor(int doctorId);
    }
}
