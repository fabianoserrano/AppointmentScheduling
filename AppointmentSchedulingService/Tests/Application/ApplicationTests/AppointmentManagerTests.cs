using Application;
using Application.Appointment;
using Application.Appointment.Dtos;
using Application.Appointment.Requests;
using Application.Doctor.Dtos;
using Domain.Entities;
using Domain.Appointment.Ports;
using Domain.Ports;
using Moq;
using Application.Patient.Dtos;

namespace ApplicationTests
{
    public class AppointmentManagerTests
    {
        AppointmentManager appointmentManager;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task HappyPath()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = DateTime.Now,
            };

            int expectedId = 444;

            var request = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            var fakeRepo = new Mock<IAppointmentRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(expectedId));

            appointmentManager = new AppointmentManager(fakeRepo.Object, doctorRepository.Object, patientRepository.Object);

            var res = await appointmentManager.CreateAppointment(request);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);

            Assert.AreEqual(res.Data.DoctorId, createAppointmentDto.DoctorId);
            Assert.AreEqual(res.Data.Date, createAppointmentDto.Date);
        }

        [Test]
        public async Task Should_Return_MissingRequiredInformation()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = DateTime.MinValue
            };

            var request = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            var fakeRepo = new Mock<IAppointmentRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(444));

            appointmentManager = new AppointmentManager(fakeRepo.Object, doctorRepository.Object, patientRepository.Object);

            var res = await appointmentManager.CreateAppointment(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [Test]
        public async Task Should_Return_DoctorNotFound()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = 999, // Assuming this is an invalid DoctorId
                Date = DateTime.Now,
            };

            doctorRepository.Setup(x => x.Get(createAppointmentDto.DoctorId)).ReturnsAsync((Doctor)null);

            var patientRepository = new Mock<IPatientRepository>();

            var request = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            var fakeRepo = new Mock<IAppointmentRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(444));

            appointmentManager = new AppointmentManager(fakeRepo.Object, doctorRepository.Object, patientRepository.Object);

            var res = await appointmentManager.CreateAppointment(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.APPOINTMENT_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "The doctor id provided was not found");
        }

        [Test]
        public async Task HappyPath_AvailableAppointments()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var appointmentRepository = new Mock<IAppointmentRepository>();

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Id,
                Date = DateTime.Now,
                PatientId = int.MinValue
            };

            var appointmentId = 1;
            var appointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = null,
            };

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = DateTime.Now,
            };

            var createAppointmentRequest = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            appointmentRepository.Setup(x => x.GetAvailableAppointments(doctorId)).ReturnsAsync(new List<Appointment>() { appointment });
            appointmentRepository.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(appointmentId));

            appointmentManager = new AppointmentManager(appointmentRepository.Object, doctorRepository.Object, patientRepository.Object);

            var resCreate = await appointmentManager.CreateAppointment(createAppointmentRequest);

            var res = await appointmentManager.GetAvailableAppointments(doctorId);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.That(1, Is.EqualTo(res.Data.Count()));
            Assert.AreEqual(res.Data[0].Id, appointmentId);
            Assert.AreEqual(res.Data[0].Date, appointmentDto.Date);

        }

        [Test]
        public async Task HappyPath_ScheduleAppointment()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var patientDto = new PatientDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
            };

            var patientId = 1;
            var patient = new Patient
            {
                Id = patientId,
                Name = patientDto.Name,
                Email = patientDto.Email,
                Password = patientDto.Password,
                CPF = patientDto.CPF,
            };

            patientRepository.Setup(x => x.Get(patientId)).ReturnsAsync(patient);

            var appointmentRepository = new Mock<IAppointmentRepository>();

            DateTime appointmentDate = DateTime.Now;

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Id,
                Date = appointmentDate,
                PatientId = int.MinValue
            };

            var appointmentId = 1;
            var appointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = null,
            };

            var updateAppointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = patient,
            };

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = appointmentDate,
            };

            var createAppointmentRequest = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            appointmentRepository.Setup(x => x.Get(appointmentId)).ReturnsAsync(appointment);
            appointmentRepository.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(appointmentId));
            appointmentRepository.Setup(x => x.Update(updateAppointment)).ReturnsAsync(appointmentId);

            var scheduleAppointmentDto = new ScheduleAppointmentDto
            {
                Id = appointmentId,
                PatientId = patientId,
            };

            var scheduleAppointmentRequest = new ScheduleAppointmentRequest()
            {
                Data = scheduleAppointmentDto,
            };

            appointmentManager = new AppointmentManager(appointmentRepository.Object, doctorRepository.Object, patientRepository.Object);

            var resCreate = await appointmentManager.CreateAppointment(createAppointmentRequest);

            var res = await appointmentManager.ScheduleAppointment(scheduleAppointmentRequest);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, scheduleAppointmentDto.Id);
            Assert.AreEqual(res.Data.PatientId, scheduleAppointmentDto.PatientId);
        }

        [Test]
        public async Task Should_Return_AppointmentAlreadyScheduled()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var patientDto = new PatientDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
            };

            var patientId = 1;
            var patient = new Patient
            {
                Id = patientId,
                Name = patientDto.Name,
                Email = patientDto.Email,
                Password = patientDto.Password,
                CPF = patientDto.CPF,
            };

            patientRepository.Setup(x => x.Get(patientId)).ReturnsAsync(patient);

            var appointmentRepository = new Mock<IAppointmentRepository>();

            DateTime appointmentDate = DateTime.Now;

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Id,
                Date = appointmentDate,
                PatientId = int.MinValue
            };

            var appointmentId = 1;
            var appointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = null,
            };

            var updateAppointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = patient,
            };

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = appointmentDate,
            };

            var createAppointmentRequest = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            appointmentRepository.Setup(x => x.Get(appointmentId)).ReturnsAsync(updateAppointment);
            appointmentRepository.Setup(x => x.Create(It.IsAny<Appointment>())).Returns(Task.FromResult(appointmentId));
            appointmentRepository.Setup(x => x.Update(updateAppointment)).ReturnsAsync(appointmentId);

            var scheduleAppointmentDto = new ScheduleAppointmentDto
            {
                Id = appointmentId,
                PatientId = patientId,
            };

            var scheduleAppointmentRequest = new ScheduleAppointmentRequest()
            {
                Data = scheduleAppointmentDto,
            };

            appointmentManager = new AppointmentManager(appointmentRepository.Object, doctorRepository.Object, patientRepository.Object);

            var resCreate = await appointmentManager.CreateAppointment(createAppointmentRequest);

            var res = await appointmentManager.ScheduleAppointment(scheduleAppointmentRequest);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.APPOINTMENT_ALREADY_SCHEDULED);
            Assert.AreEqual(res.Message, "Appointment already scheduled");
        }

        [Test]
        public async Task Should_Return_AppointmentTimeOverlapping()
        {
            var doctorRepository = new Mock<IDoctorRepository>();

            var doctorDto = new DoctorDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
                CRM = "CRM/SP 123456",
            };

            var doctorId = 1;
            var doctor = new Doctor
            {
                Id = doctorId,
                Name = doctorDto.Name,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                CPF = doctorDto.CPF,
                CRM = doctorDto.CRM,
            };

            doctorRepository.Setup(x => x.Get(doctorId)).ReturnsAsync(doctor);

            var patientRepository = new Mock<IPatientRepository>();

            var patientDto = new PatientDto
            {
                Name = "Fulano",
                Email = "fulano@gmail.com",
                Password = "password",
                CPF = "12345678901",
            };

            var patientId = 1;
            var patient = new Patient
            {
                Id = patientId,
                Name = patientDto.Name,
                Email = patientDto.Email,
                Password = patientDto.Password,
                CPF = patientDto.CPF,
            };

            patientRepository.Setup(x => x.Get(patientId)).ReturnsAsync(patient);

            var appointmentRepository = new Mock<IAppointmentRepository>();

            DateTime appointmentDate = DateTime.Now;
            DateTime newAppointmentDate = DateTime.Now.AddMinutes(20);

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Id,
                Date = appointmentDate,
                PatientId = int.MinValue
            };

            var appointmentId = 1;
            var appointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = null,
            };

            var updateAppointment = new Appointment
            {
                Id = appointmentId,
                Doctor = doctor,
                Date = appointmentDto.Date,
                Patient = patient,
            };

            var createAppointmentDto = new CreateAppointmentDto
            {
                DoctorId = doctorId,
                Date = newAppointmentDate,
            };

            var createAppointmentRequest = new CreateAppointmentRequest()
            {
                Data = createAppointmentDto,
            };

            appointmentRepository.Setup(x => x.GetAppointments(doctorId, newAppointmentDate)).ReturnsAsync(new List<Appointment>() { appointment });

            var scheduleAppointmentDto = new ScheduleAppointmentDto
            {
                Id = appointmentId,
                PatientId = patientId,
            };

            var scheduleAppointmentRequest = new ScheduleAppointmentRequest()
            {
                Data = scheduleAppointmentDto,
            };

            appointmentManager = new AppointmentManager(appointmentRepository.Object, doctorRepository.Object, patientRepository.Object);

            var res = await appointmentManager.CreateAppointment(createAppointmentRequest);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.APPOINTMENT_TIME_OVERLAPPING);
            Assert.AreEqual(res.Message, "Overlapping appointment hours");
        }
    }
}