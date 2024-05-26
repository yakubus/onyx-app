using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Shared.Constants
{
    internal static class DateTimeConstants
    {
        internal static readonly DateTime MinimumValidPastDateTime = new(
            DateTime.UtcNow.Year - 1,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day);
    }
}
