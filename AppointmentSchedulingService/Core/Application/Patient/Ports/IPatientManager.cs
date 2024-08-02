using Application.Patient.Requests;
using Application.Patient.Responses;

namespace Application.Patient.Ports
{
    public interface IPatientManager
    {
        Task<PatientResponse> CreatePatient(CreatePatientRequest request);
        Task<PatientResponse> GetPatient(int patientId);
        Task<PatientResponse> UpdatePatient(UpdatePatientRequest request);
        Task<PatientResponse> DeletePatient(int patientId);
    }
}
