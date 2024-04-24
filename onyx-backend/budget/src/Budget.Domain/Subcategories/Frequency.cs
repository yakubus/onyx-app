namespace Budget.Domain.Subcategories;

public sealed record Frequency
{
    public string Value { get; init; }

    private Frequency(string value) => Value = value;

    public static Frequency Once => new(nameof(Once));
    public static Frequency Weekly => new(nameof(Weekly));
    public static Frequency Monthly => new(nameof(Monthly));
    public static Frequency Yearly => new(nameof(Yearly));

}