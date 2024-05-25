using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Functions.Models
{
    public sealed record EmailData
    {
        public string Recipient { get; init; }
        public string Subject { get; init; }
        public string HtmlBody { get; init; }
        public string PlainTextBody { get; init; }

        private EmailData(string recipient, string subject, string htmlBody, string plainTextBody)
        {
            Recipient = recipient;
            Subject = subject;
            HtmlBody = htmlBody;
            PlainTextBody = plainTextBody;
        }
    }
}
