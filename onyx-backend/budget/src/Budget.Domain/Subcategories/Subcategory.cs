using Budget.Domain.Transactions;
using Models.DataTypes;

namespace Budget.Domain.Subcategories
{
    public class Subcategory
    {
        public IReadOnlyCollection<Transaction> Transactions => _transactions;
        private List<Transaction> _transactions;
        public IReadOnlyCollection<Target> Targets => _targets;
        private List<Target> _targets;
    }
}
