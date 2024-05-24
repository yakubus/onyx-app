using System.Reflection;

namespace Identity.Application;

internal sealed class ApplicationAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}