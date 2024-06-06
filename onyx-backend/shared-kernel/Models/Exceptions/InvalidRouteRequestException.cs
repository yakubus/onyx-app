using System.Text.RegularExpressions;

namespace Models.Exceptions;

public sealed class InvalidRouteRequestException : Exception
{
    public InvalidRouteRequestException(
        string path,
        Regex pathRegex) : base($"Route '{path}' does not contain required parameter '{pathRegex.GroupNameFromNumber(0)}'")
    {
    }
}