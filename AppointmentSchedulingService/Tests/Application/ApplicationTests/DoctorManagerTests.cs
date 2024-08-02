using Application;
using Application.Doctor;
using Application.Doctor.Dtos;
using Application.Doctor.Requests;
using Domain.Entities;
using Domain.Ports;
using Moq;

namespace ApplicationTests
{
    public class DoctorManagerTests
    {
        DoctorManager doctorManager;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task HappyPath()
        {
            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456"
            };

            int expectedId = 222;

            var request = new CreateDoctorRequest()
            {
                Data = doctorDto,
            };

            var fakeRepo = new Mock<IDoctorRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Doctor>())).Returns(Task.FromResult(expectedId));

            doctorManager = new DoctorManager(fakeRepo.Object);

            var res = await doctorManager.CreateDoctor(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);
            Assert.AreEqual(res.Data.Name, doctorDto.Name);
            Assert.AreEqual(res.Data.CPF, doctorDto.CPF);
            Assert.AreEqual(res.Data.CRM, doctorDto.CRM);
        }

        [TestCase("", "abcd@email.com", "abcd1234", "12345678901", "CRM/SP 123456")]
        [TestCase(null, "abcd@email.com", "abcd1234", "12345678901", "CRM/SP 123456")]
        [TestCase("Fulano", "", "abcd1234", "12345678901", "CRM/SP 123456")]
        [TestCase("Fulano", null, "abcd1234", "12345678901", "CRM/SP 123456")]
        [TestCase("Fulano", "abcd@email.com", "", "12345678901", "CRM/SP 123456")]
        [TestCase("Fulano", "abcd@email.com", null, "12345678901", "CRM/SP 123456")]
        [TestCase("", "", "", "12345678901", "CRM/SP 123456")]
        [TestCase(null, null, null, "12345678901", "CRM/SP 123456")]
        [TestCase("Fulano", "abcd@email.com", "abcd1234", "", "CRM/SP 123456")]
        [TestCase("Fulano", "abcd@email.com", "abcd1234", "12345678901", "")]
        [TestCase("Fulano", "abcd@email.com", "abcd1234", "12345678901", null)]
        public async Task Should_Return_MissingRequiredInformation_WhenDocsAreInvalid(string name, string email, string password, string cpf, string crm)
        {
            var doctorDto = new DoctorDto
            {
                Name = name,
                Email = email,
                Password = password,
                CPF = cpf,
                CRM = crm
            };

            var request = new CreateDoctorRequest()
            {
                Data = doctorDto,
            };

            var fakeRepo = new Mock<IDoctorRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Doctor>())).Returns(Task.FromResult(222));

            doctorManager = new DoctorManager(fakeRepo.Object);

            var res = await doctorManager.CreateDoctor(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [TestCase("b@b.com")]
        [TestCase("emailsemarrobaesemponto")]
        public async Task Should_Return_InvalidEmailExceptions_WhenEmailAreInvalid(string email)
        {
            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = email,
                Password = "abcd1234",
                CPF = "12345678901",
                CRM = "CRM/SP 123456"
            };

            var request = new CreateDoctorRequest()
            {
                Data = doctorDto,
            };

            var fakeRepo = new Mock<IDoctorRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Doctor>())).Returns(Task.FromResult(222));

            doctorManager = new DoctorManager(fakeRepo.Object);

            var res = await doctorManager.CreateDoctor(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(res.Message, "The given email is not valid");
        }
    }
}