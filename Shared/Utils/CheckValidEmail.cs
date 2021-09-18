using System.Text.RegularExpressions;

namespace Shared.Utils
{
    public static class CheckValidEmail
    {
        public static bool Validate(string email)
        {
            return Regex.IsMatch(email,
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}