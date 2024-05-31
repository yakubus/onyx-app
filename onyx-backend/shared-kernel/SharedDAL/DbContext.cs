using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace SharedDAL;

public sealed class DbContext
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public DbContext()
    {
        _client = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(_client);
    }

}