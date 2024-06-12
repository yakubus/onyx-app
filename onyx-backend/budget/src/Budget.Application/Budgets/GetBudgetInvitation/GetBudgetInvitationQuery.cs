using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.GetBudgetInvitation;

public sealed record GetBudgetInvitationQuery(Guid BudgetId, string BaseUrl) : IQuery<InvitationUrl>
{
}