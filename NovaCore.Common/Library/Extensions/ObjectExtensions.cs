using System;

namespace NovaCore.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj)
        {
            if (!obj.GetType().IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException();
            }

            return (T)obj;
        }
    }
}