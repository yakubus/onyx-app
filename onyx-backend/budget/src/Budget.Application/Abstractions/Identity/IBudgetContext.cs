using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Application.Abstractions.Identity;

public interface IBudgetContext
{
    Result<Guid> GetBudgetId();
}