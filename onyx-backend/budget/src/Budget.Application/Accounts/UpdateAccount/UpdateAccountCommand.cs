﻿using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Application.Shared.Models;

namespace Budget.Application.Accounts.UpdateAccount;

public sealed record UpdateAccountCommand(Guid Id, string? NewName, MoneyModel? NewBalance) : ICommand<AccountModel>
{
}