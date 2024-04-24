using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Exceptions;

public sealed class DomainException<T>(string message) : DomainException(message, typeof(T));

public abstract class DomainException(string message, Type type) : Exception(message)
{
    public Type Type { get; init; } = type;
}