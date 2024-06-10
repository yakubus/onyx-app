using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Application.Budgets.Models;

public sealed record InvitationUrl(string Value, int ValidForSeconds);