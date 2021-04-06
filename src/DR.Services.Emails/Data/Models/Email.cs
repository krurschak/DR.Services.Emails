using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static DR.Components.Emails.Enumeration;

namespace DR.Services.Emails.Data.Models
{
    [Table("Emails")]
    public class Email
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? AppId { get; set; }

        [Required]
        public string Recipients { get; set; }

        public string Cc { get; set; }
        public string Bcc { get; set; }

        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        public string Signature { get; set; }

        public EmailStatus EmailStatus { get; set; }
        public DateTime? SentDateUtc { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime CreateDateUtc { get; set; }
        public DateTime LastUpdateUtc { get; set; }
    }
}
