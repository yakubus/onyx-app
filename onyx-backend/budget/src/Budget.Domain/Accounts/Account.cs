using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Accounts
{
    public abstract class Account : Entity<AccountId>
    {
    }
}
