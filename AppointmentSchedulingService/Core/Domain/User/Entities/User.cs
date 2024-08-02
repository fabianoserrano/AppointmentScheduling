using Domain.Ports;
using Domain.User;
using Domain.User.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CPF { get; set; }

        protected virtual void ValidateState()
        {
            if (string.IsNullOrEmpty(Name) || 
                string.IsNullOrEmpty(Email) || 
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(CPF))
            {
                throw new MissingRequiredInformation();
            }
            if (Utils.ValidateEmail(this.Email) == false)
            {
                throw new InvalidEmailException();
            }
        }

        public async Task Save(IUserRepository userRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                this.Id = await userRepository.Create(this);
            }
            else
            {
                await userRepository.Update(this);
            }
        }
    }
}
