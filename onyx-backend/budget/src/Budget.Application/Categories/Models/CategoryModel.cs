using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Categories;

namespace Budget.Application.Categories.Models;

public sealed record CategoryModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<SubcategoryModel> Subcategories { get; init; }

    [JsonConstructor]
    public CategoryModel(
        Guid id,
        string name,
        List<SubcategoryModel> subcategories,
        IEnumerable<IDomainEvent> domainEvents) 
        : base(domainEvents)
    {
        Id = id;
        Name = name;
        Subcategories = subcategories;
    }


    internal static CategoryModel FromDomainModel(Category domainModel) =>
        new(domainModel.Id.Value,
            domainModel.Name.Value,
            domainModel.Subcategories
                .Select(SubcategoryModel.FromDomainModel)
                .ToList(),
            domainModel.GetDomainEvents());
}