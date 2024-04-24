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
    public sealed record Target : ValueObject
    {
        public IReadOnlyCollection<Transaction> Transactions => _transactions;
        private List<Transaction> _transactions;
        public Frequency Frequency { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        private Target(Frequency frequency, DateTime startDate, DateTime endDate)
        {
            Frequency = frequency;
            StartDate = startDate;
            EndDate = endDate;
            _transactions = new();
        }

        private Target()
        { }

        /*
         * TODO
         * - Create method for initializing target
         * - Recreate method for creating new target based on previous one (rather put into Subcategory
         * - 
        */
    }
}
