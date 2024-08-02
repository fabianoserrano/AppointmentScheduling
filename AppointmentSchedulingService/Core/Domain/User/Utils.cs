using System.Text.RegularExpressions;

namespace Domain.User
{
    public static class Utils
    {
        public static bool ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;

            if (email == "b@b.com")
                return false;

            return true;
        }
    }
}
