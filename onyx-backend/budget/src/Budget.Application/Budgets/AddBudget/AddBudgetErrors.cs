using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Application.Budgets.AddBudget
{
    internal static class AddBudgetErrors
    {
        public static readonly Error BudgetNameNotUnique = new Error(
            "AddBudget.BudgetNameNotUnique", 
            "Budget with this name already exists");
    }
}
