using Amazon.DynamoDBv2.DocumentModel;

namespace SharedDAL.Extensions;

public static class DynamoDbEntryExtensions
{
    public static string? AsNullableString(this DynamoDBEntry entry) =>
        Equals(entry, DynamoDBNull.Null) ? null : entry.AsString();

    public static int? AsNullableInt(this DynamoDBEntry entry) =>
        Equals(entry, DynamoDBNull.Null) ? null : entry.AsInt();

    public static decimal? AsNullableDecimal(this DynamoDBEntry entry) =>
        Equals(entry, DynamoDBNull.Null) ? null : entry.AsDecimal();
}