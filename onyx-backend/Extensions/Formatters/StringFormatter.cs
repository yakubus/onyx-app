using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Formatters;

public static class StringFormatter
{
    public static string Capitalize(this string value) =>
        string.Concat(
            value[0].ToString().ToUpper(),
            value[1..].ToLower());
}