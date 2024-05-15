using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Models;

public sealed record AuthorizationToken
{
    public string AccessToken { get; init; }

    public AuthorizationToken(string accessToken)
    {
        AccessToken = accessToken;
    }
}