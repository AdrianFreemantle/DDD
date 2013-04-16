using System;
using System.ComponentModel;
using System.Reflection;

namespace PersistenceModel.Reporting
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            try
            {
                var field = GetEnumFieldInfo(value);
                return GetFieldDescription(field);
            }
            catch (Exception)
            {
                return "Error! Unable to find a name for this field.";
            }
        }

        public static T TryGetCustomAttribute<T>(this Enum value) where T : Attribute
        {
            var field = GetEnumFieldInfo(value);
            return Attribute.GetCustomAttribute(field, typeof(T)) as T;
        }

        private static FieldInfo GetEnumFieldInfo(Enum value)
        {
            Type type = value.GetType();
            var name = Enum.GetName(type, value);
            var field = type.GetField(name);
            return field;
        }

        private static string GetFieldDescription(FieldInfo field)
        {
            string name = field.Name;
            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (!ReferenceEquals(null, attr))
            {
                name = attr.Description;
            }

            return name;
        }
    }
}