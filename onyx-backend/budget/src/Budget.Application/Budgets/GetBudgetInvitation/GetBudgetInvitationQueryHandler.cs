using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Budget.Application.Budgets.Models;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Budgets.GetBudgetInvitation;

internal sealed class GetBudgetInvitationQueryHandler : IQueryHandler<GetBudgetInvitationQuery, InvitationUrl>
{
    private readonly IBudgetRepository _budgetRepository;

    private string GetInvitationRelativePath(string token, Guid budgetId) =>
        $"/budgets/join?token={token}&budgetId={budgetId}";

    public GetBudgetInvitationQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<Result<InvitationUrl>> Handle(GetBudgetInvitationQuery request, CancellationToken cancellationToken)
    {
        var getBudgetResult = await _budgetRepository.GetByIdAsync(new BudgetId(request.BudgetId), cancellationToken);

        if (getBudgetResult.IsFailure)
        {
            return getBudgetResult.Error;
        }

        var budget = getBudgetResult.Value;

        var token = budget.GetInvitationToken();
        var tokenValidForSeconds = (token.ExpirationDate - DateTime.UtcNow).Seconds;

        var url = string.Join(request.BaseUrl, GetInvitationRelativePath(token.Value, budget.Id.Value));

        var updateResult = await _budgetRepository.UpdateAsync(budget, cancellationToken);

        if(updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        return new InvitationUrl(url, tokenValidForSeconds);
    }
}