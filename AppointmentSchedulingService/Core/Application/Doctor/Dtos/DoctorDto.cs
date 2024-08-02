using Application.User.Dtos;
using Entities = Domain.Entities;

namespace Application.Doctor.Dtos
{
    public class DoctorDto : UserDto
    {
        public string CRM { get; set; }
        public static Entities.Doctor MapToEntity(DoctorDto doctorDto)
        {
            return new Entities.Doctor
            {
                Id = doctorDto.Id,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };
        }
        public static DoctorDto MapToDto(Entities.Doctor doctor) 
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Email = doctor.Email,
                Password = doctor.Password,
                CPF = doctor.CPF,
                CRM = doctor.CRM,
            };
        }
    }
}
