using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Functions.Middlewares;

internal static class Interceptor
{
    public static Task<IActionResult> InterceptFunction(Func<Task<IActionResult>> function)
    {
        return function.Invoke();
    }
}