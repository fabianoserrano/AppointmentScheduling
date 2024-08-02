using Application;
using Application.Doctor.Dtos;
using Application.User;
using Application.User.Dtos;
using Application.User.Requests;
using Domain.Entities;
using Domain.Ports;
using Moq;

namespace ApplicationTests
{
    public class UserManagerTests
    {
        UserManager userManager;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task HappyPath()
        {
            var userDto = new UserDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901"
            };

            int expectedId = 222;

            var request = new CreateUserRequest()
            {
                Data = userDto,
            };

            var fakeRepo = new Mock<IUserRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.FromResult(expectedId));

            userManager = new UserManager(fakeRepo.Object);

            var res = await userManager.CreateUser(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);
            Assert.AreEqual(res.Data.Name, userDto.Name);
            Assert.AreEqual(res.Data.CPF, userDto.CPF);
        }

        [TestCase("", "abcd@email.com", "abcd1234", "12345678901")]
        [TestCase(null, "abcd@email.com", "abcd1234", "12345678901")]
        [TestCase("Fulano", "", "abcd1234", "12345678901")]
        [TestCase("Fulano", null, "abcd1234", "12345678901")]
        [TestCase("Fulano", "abcd@email.com", "", "12345678901")]
        [TestCase("Fulano", "abcd@email.com", null, "12345678901")]
        [TestCase("", "", "", "12345678901")]
        [TestCase(null, null, null, "12345678901")]
        [TestCase("Fulano", "abcd@email.com", "abcd1234", "")]
        public async Task Should_Return_MissingRequiredInformation_WhenDocsAreInvalid(string name, string email, string password, string cpf)
        {
            var userDto = new UserDto
            {
                Name = name,
                Email = email,
                Password = password,
                CPF = cpf,
            };

            var request = new CreateUserRequest()
            {
                Data = userDto,
            };

            var fakeRepo = new Mock<IUserRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.FromResult(222));

            userManager = new UserManager(fakeRepo.Object);

            var res = await userManager.CreateUser(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [TestCase("b@b.com")]
        [TestCase("emailsemarrobaesemponto")]
        public async Task Should_Return_InvalidEmailExceptions_WhenEmailAreInvalid(string email)
        {
            var userDto = new UserDto
            {
                Name = "Fulano",
                Email = email,
                Password = "abcd1234",
                CPF = "12345678901"
            };

            var request = new CreateUserRequest()
            {
                Data = userDto,
            };

            var fakeRepo = new Mock<IUserRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.FromResult(222));

            userManager = new UserManager(fakeRepo.Object);

            var res = await userManager.CreateUser(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(res.Message, "The given email is not valid");
        }
    }
}