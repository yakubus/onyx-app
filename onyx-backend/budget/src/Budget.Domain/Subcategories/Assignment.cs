using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories
{
    // TODO: make two gets detailed with transactions, simple wihout transactions
    public sealed record Assignment : ValueObject
    {
        public MonthDate Month { get; init; }
        public Money AssignedAmount { get; private set; }
        public Money ActualAmount { get; init; }
        private readonly List<Transaction> _transactions;
        public IReadOnlyList<Transaction> Transactions => _transactions;

        private Assignment(MonthDate month, Money assignedAmount, Money actualAmount, List<Transaction> transactions)
        {
            Month = month;
            AssignedAmount = assignedAmount;
            ActualAmount = actualAmount;
            _transactions = transactions;
        }

        public Result ChangeAssignedAmount(Money amount)
        {
            if (amount <= 0)
            {
                return Result.Failure<Assignment>(SubcategoryErrors.AssignmentAmountMustBePositive);
            }

            AssignedAmount = amount;

            return Result.Success();
        }

        public static Result<Assignment> Create(MonthDate month, Money assignedAmount)
        {
            if(assignedAmount <= 0)
            {
                return Result.Failure<Assignment>(SubcategoryErrors.AssignmentAmountMustBePositive);
            }

            if (MonthDate.Current > month || MonthDate.Current + 1 < month)
            {
                return Result.Failure<Assignment>(SubcategoryErrors.AssignmentDateMustBeInNextOrCurrentMonth);
            }

            return new Assignment(
                month,
                assignedAmount,
                assignedAmount with { Amount = 0 },
                new List<Transaction>());
        }
    }
}
