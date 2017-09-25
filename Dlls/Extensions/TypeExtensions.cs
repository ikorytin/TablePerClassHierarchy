using System;

using Data.Core.Domain.Model.Entities;

namespace Data.Core.Domain.Extensions
{
    public static class TypeExtensions
    {
        public static bool InheritedFromBaseEntity(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type.BaseType == typeof(BaseEntity))
            {
                return true;
            }

            if (type.BaseType == typeof(object))
            {
                return false;
            }

            return InheritedFromBaseEntity(type.BaseType);
        } 
    }
}