using Domain.Ports;
using Domain.User.Exceptions;

namespace Domain.Entities
{
    public class Doctor : User
    {
        public string CRM { get; set; }

        protected override void ValidateState()
        {
            base.ValidateState();

            if (string.IsNullOrEmpty(CRM))
            {
                throw new MissingRequiredInformation();
            }
        }

        public async Task Save(IDoctorRepository doctorRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                this.Id = await doctorRepository.Create(this);
            }
            else
            {
                await doctorRepository.Update(this);
            }
        }
    }
}
