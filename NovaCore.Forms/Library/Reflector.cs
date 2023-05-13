using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NovaCore.Forms;

internal class Reflector
{
    #region Data Members

    private readonly string? _namespace;

    private readonly Assembly? _assembly;
    
    private const BindingFlags _BINDINGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    #endregion

    #region Attributes

    private static IEnumerable<AssemblyName> ReferencedAssemblies => 
        Assembly.GetExecutingAssembly().GetReferencedAssemblies();

    #endregion

    #region Constructor

    public Reflector(string @namespace) : this(@namespace, @namespace)
    {
    }

    private Reflector(string assembly, string @namespace)
    {
        _namespace = @namespace;
        _assembly = null;
        
        foreach (AssemblyName assemblyName in ReferencedAssemblies)
        {
            if (!assemblyName.FullName.StartsWith(assembly))
                continue;
            _assembly = Assembly.Load(assemblyName);
            break;
        }
    }

    #endregion

    public Type? GetType(string typeName)
    {
        if (_assembly is null)
            return null;

        string[] names = typeName.Split('.');

        if (names.Length <= 0)
            return null;

        return names.Skip(1).Aggregate(_assembly.GetType($"{_namespace}.{names[0]}"), (current, name) =>
        {
            return current?.GetNestedType(name, BindingFlags.NonPublic);
        });

    }
    
    public object? New(string name, params object[] parameters)
    {
        if (GetType(name) is not { } type || type.GetConstructors() is not { } constructorInfos)
            return null;
        
        foreach (ConstructorInfo constructorInfo in constructorInfos)
        {
            try { return constructorInfo.Invoke(parameters); }
            catch { /* ignored */ }
        }

        return null;
    }
    
    public static object? Call(object obj, string func, params object[] parameters)
    {
        return CallAs(obj.GetType(), obj, func, parameters);
    }

    public static object? CallAs(Type type, object obj, string func, params object[] parameters)
    {
        MethodInfo? methodInfo = type.GetMethod(func, _BINDINGS);
        return methodInfo?.Invoke(obj, parameters);
    }
    
    public static object? Get(object obj, string prop)
    {
        return GetAs(obj.GetType(), obj, prop);
    }
    
    public static object? GetAs(IReflect type, object obj, string prop)
    {
        return type.GetProperty(prop, _BINDINGS)?.GetValue(obj, null);
    }
    
    public object? GetEnum(string typeName, string name)
    {
        if (GetType(typeName) is not { } type || type.GetField(name) is not { } fieldInfo)
            return null;
        return fieldInfo.GetValue(null);
    }
}