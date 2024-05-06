using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.UpdateSubcategory;

internal sealed class UpdateSubcategoryCommandHandler : ICommandHandler<UpdateSubcategoryCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public UpdateSubcategoryCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }

    // TODO Update subcateories in related transactions
    public async Task<Result<SubcategoryModel>> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.Id);
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var updateNameResult = UpdateName(subcategory, request.NewName);

        if (updateNameResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(updateNameResult.Error);
        }

        var updateDescriptionResult = UpdateDescription(subcategory, request.NewDescription);

        if (updateDescriptionResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(updateDescriptionResult.Error);
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryUpdateResult.Error);
        }

        subcategory = subcategoryUpdateResult.Value;

        return SubcategoryModel.FromDomainModel(subcategory);
    }

    private Result<Subcategory> UpdateName(Subcategory subcategory, string? newName)
    {
        if (newName is null)
        {
            return Result.Success(subcategory);
        }

        var changeNameResult = subcategory.ChangeName(newName);

        return changeNameResult.IsFailure ?
            Result.Failure<Subcategory>(changeNameResult.Error) :
            Result.Success(subcategory);
    }

    private Result<Subcategory> UpdateDescription(Subcategory subcategory, string? newDescription)
    {
        if (newDescription is null)
        {
            return Result.Success(subcategory);
        }

        var changeDescriptionResult = subcategory.ChangeDescription(newDescription);

        return changeDescriptionResult.IsFailure ?
            Result.Failure<Subcategory>(changeDescriptionResult.Error) :
            Result.Success(subcategory);
    }
}