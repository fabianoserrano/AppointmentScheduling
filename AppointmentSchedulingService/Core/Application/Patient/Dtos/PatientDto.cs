using Application.User.Dtos;
using Entities = Domain.Entities;

namespace Application.Patient.Dtos
{
    public class PatientDto : UserDto
    {
        public static Entities.Patient MapToEntity(PatientDto patientDto)
        {
            return new Entities.Patient
            {
                Id = patientDto.Id,
                Name = patientDto.Name,
                Email = patientDto.Email,
                Password = patientDto.Password,
                CPF = patientDto.CPF,
            };
        }
        public static PatientDto MapToDto(Entities.Patient patient) 
        {
            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email,
                Password = patient.Password,
                CPF = patient.CPF,
            };
        }
    }
}
