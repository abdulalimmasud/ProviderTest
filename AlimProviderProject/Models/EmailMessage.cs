using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlimProviderProject.Models
{
    public class EmailMessage
    {
        public string ReceivedDateTime { get; set; }
        public string SentDateTime { get; set; }
        public bool HasAttachments { get; set; } 
        public string Subject { get; set; }
        public string Importance { get; set; }
        public EmailAddress From { get; set; }
        public EmailBody Body { get; set; }
        public List<EmailAddress> ToRecipients { get; set; }
        public List<EmailAddress> CcRecipients { get; set; }
        public List<EmailAddress> BccRecipients { get; set; }
        public bool IsRead { get; set; }
        public string Id { get; set; }
          
    }
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class EmailBody
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
