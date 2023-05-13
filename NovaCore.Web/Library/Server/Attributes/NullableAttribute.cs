using System;

namespace NovaCore.Web.Server.Attributes
{
    /// <summary>
    /// Marks a controller method argmuent 
    /// as an argument that may be null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NullableAttribute : Attribute { }
}