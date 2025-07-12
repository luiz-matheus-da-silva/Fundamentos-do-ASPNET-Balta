using System.Net.NetworkInformation;

namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey { get; set; } = "nkqB0FS7The1XdDI6fp7ksT1rDLAcRGqZbnFMzYJoQ4=";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "blog_api_IlTevUM/z0ey3NwCV/unWg==";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
