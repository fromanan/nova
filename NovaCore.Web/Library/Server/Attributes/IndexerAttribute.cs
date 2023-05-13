using System;

namespace NovaCore.Web.Server.Attributes;

public class IndexerAttribute : Attribute
{
    public IndexerAttribute(int precedence = 0)
    {
        Precedence = precedence;
    }

    public int Precedence { get; }
}