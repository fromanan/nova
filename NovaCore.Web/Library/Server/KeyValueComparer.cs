using System;
using System.Collections.Generic;

namespace NovaCore.Web.Server;

public class KeyValueComparer<TKey, TValue, TOutput> : IEqualityComparer<KeyValuePair<TKey, TValue>>
{
    private readonly Func<KeyValuePair<TKey, TValue>, TOutput> _outputFunc;
        
    private readonly IEqualityComparer<TOutput> _outputComparer;
        
    public KeyValueComparer(Func<KeyValuePair<TKey, TValue>, TOutput> outputFunc,
        IEqualityComparer<TOutput> outputComparer)
    {
        _outputFunc = outputFunc;
        _outputComparer = outputComparer;
    }

    public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
    {
        return _outputComparer.Equals(_outputFunc(x), _outputFunc(y));
    }

    public int GetHashCode(KeyValuePair<TKey, TValue> obj)
    {
        return _outputComparer.GetHashCode(_outputFunc(obj));
    }
}