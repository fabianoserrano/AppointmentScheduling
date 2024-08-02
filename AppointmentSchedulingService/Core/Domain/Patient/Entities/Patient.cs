using Domain.Ports;

namespace Domain.Entities
{
    public class Patient : User
    {
        public async Task Save(IPatientRepository patientRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                this.Id = await patientRepository.Create(this);
            }
            else
            {
                await patientRepository.Update(this);
            }
        }
    }
}
