using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;

namespace Budget.Application.Categories.Models;

public sealed record CategoryModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<SubcategoryModel> Subcategories { get; init; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    public CategoryModel(
        Guid id,
        string name,
        IEnumerable<SubcategoryModel> subcategories,
        IEnumerable<IDomainEvent> domainEvents) 
        : base(domainEvents)
    {
        Id = id;
        Name = name;
        Subcategories = subcategories;
    }


    internal static CategoryModel FromDomainModel(Category domainModel, IEnumerable<Subcategory> subcategories) =>
        new(domainModel.Id.Value,
            domainModel.Name.Value,
            subcategories.Select(SubcategoryModel.FromDomainModel),
            domainModel.GetDomainEvents());
}