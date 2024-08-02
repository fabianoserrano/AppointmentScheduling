using Application.User.Dtos;
using Entities = Domain.Entities;

namespace Application.Doctor.Dtos
{
    public class DoctorNoPasswordDto : UserDto
    {
        public string CRM { get; set; }
        public static DoctorNoPasswordDto MapToDto(Entities.Doctor doctor) 
        {
            return new DoctorNoPasswordDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Email = doctor.Email,
                CPF = doctor.CPF,
                CRM = doctor.CRM,
            };
        }
    }
}
