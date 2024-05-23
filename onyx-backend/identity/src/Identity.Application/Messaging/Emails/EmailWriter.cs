using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain;

namespace Identity.Application.Messaging.Emails
{
    internal sealed class EmailWriter
    {
        private readonly IEmailService _emailService;
        private readonly string _email;
        private readonly string _subject;
        private readonly string _body;
    }
}
