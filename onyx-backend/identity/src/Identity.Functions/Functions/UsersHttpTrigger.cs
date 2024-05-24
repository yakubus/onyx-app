using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Functions.Functions;

public sealed class UsersHttpTrigger
{
    private readonly ISender _sender;

    public UsersHttpTrigger(ISender sender)
    {
        _sender = sender;
    }


    [FunctionName("GetUser")]
    public static async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
    {
        var userId = req.Query["id"];
        var email = req.Query["email"];

        return new OkObjectResult("Hello World!");
    }
}