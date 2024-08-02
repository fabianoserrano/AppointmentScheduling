using Entities = Domain.Entities;

namespace Application.User.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CPF { get; set; }
        public static Entities.User MapToEntity(UserDto userDto)
        {
            return new Entities.User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                CPF = userDto.CPF,
            };
        }
        public static UserDto MapToDto(Entities.User user) 
        {
            return new UserDto 
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CPF = user.CPF,
            };
        }
    }
}
