using System.Diagnostics.CodeAnalysis;

namespace Cerium.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CeriumRouteAttribute(string method, [StringSyntax("Route")] string path) : Attribute
{
    public string Method = method;
    public string Path = path;
}