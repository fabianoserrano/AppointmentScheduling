namespace Application
{
    public enum ErrorCodes
    {
        // Users related codes 1 to 99
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        MISSING_REQUIRED_INFORMATION = 3,
        INVALID_EMAIL = 4,
        USER_NOT_FOUND = 5,
        USER_UPDATE_FAILED = 6,
        USER_DELETE_FAILED = 7,

        // Doctor related codes 100 to 199
        DOCTOR_NOT_FOUND = 100,
        DOCTOR_UPDATE_FAILED = 101,
        DOCTOR_DELETE_FAILED = 102,

        // Patient related codes 200 to 299
        PATIENT_NOT_FOUND = 200,
        PATIENT_UPDATE_FAILED = 201,
        PATIENT_DELETE_FAILED = 202,

        // Appointment related codes 300 to 399
        APPOINTMENT_NOT_FOUND = 300,
        APPOINTMENT_COULD_NOT_STORE_DATA = 301,
        APPOINTMENT_MISSING_REQUIRED_INFORMATION = 302,
        APPOINTMENT_UPDATE_FAILED = 303,
        APPOINTMENT_DELETE_FAILED = 304,
        APPOINTMENT_ALREADY_SCHEDULED = 305,
        APPOINTMENT_TIME_OVERLAPPING = 306,

        // Email related codes 400 to 499
        EMAIL_MISSING_REQUIRED_INFORMATION = 400,
        EMAIL_COULD_NOT_STORE_DATA = 401,
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
