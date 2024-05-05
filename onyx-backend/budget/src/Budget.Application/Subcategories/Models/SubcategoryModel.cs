using System.Text.Json.Serialization;
using Budget.Domain.Subcategories;

namespace Budget.Application.Subcategories.Models;

public sealed record SubcategoryModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public List<AssignmentModel> Assignments { get; init; }
    public TargetModel? Target { get; init; }

    [JsonConstructor]
    private SubcategoryModel(
        Guid id,
        string name,
        string? description,
        List<AssignmentModel> assignments,
        TargetModel? target)
    {
        Id = id;
        Name = name;
        Description = description;
        Assignments = assignments;
        Target = target;
    }

    internal static SubcategoryModel FromDomainModel(Subcategory domainModel) =>
        new(domainModel.Id.Value,
            domainModel.Name.Value,
            domainModel.Description?.Value,
            domainModel.Assignments.Select(AssignmentModel.FromDomainModel).ToList(),
            domainModel.Target is null 
                ? null 
                : TargetModel.FromDomainModel(domainModel.Target));
}