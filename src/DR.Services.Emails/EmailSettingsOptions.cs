using System;

namespace DR.Services.Emails
{
    [Serializable]
    public class EmailSettingsOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string FromName { get; set; }
        public string Signature { get; set; }
        public string TestRecipients { get; set; }
    }
}
