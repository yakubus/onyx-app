using Microsoft.Identity.Client;

namespace Budget.API.Controllers.Categories.Requests;

public sealed record AddCategoryRequest
{
    public string Name { get; set; }

    private AddCategoryRequest(string name)
    {
        Name = name;
    }
}