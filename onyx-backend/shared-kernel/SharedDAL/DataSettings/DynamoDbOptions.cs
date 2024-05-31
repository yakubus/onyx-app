namespace SharedDAL.DataSettings;

public sealed class DynamoDbOptions
{
    public DynamoDbTables Tables { get; set; }

    public sealed class DynamoDbTables
    {
        public string Accounts { get; set; }
        public string Budgets { get; set; }
        public string Users { get; set; }
        public string Subcategories { get; set; }
        public string Categories { get; set; }
        public string Transactions { get; set; }
        public string Counterparties { get; set; }
    }
}