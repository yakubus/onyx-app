using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DataTypes;

namespace Budget.Application.Abstractions.Services;

public interface ICurrencyConverter
{
    Task<Money> ConvertMoney(Money amount, Currency destinationCurrency, CancellationToken  cancellationToken = default);
}