using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Domain.Shared.Errors
{
    internal static class DateTimeErrors
    {
        internal static readonly Error InvalidDateTime = new (
            "DateTime.Invalid",
            "The date is invalid"
        );

    }
}
