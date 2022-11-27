using System.ComponentModel;
using System.Reflection;

namespace enum_example.helpers;

public static class EnumHelper
{
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType()
                            .GetField(value.ToString());
        
        var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute)) as DescriptionAttribute[];

        return attributes.First().Description;
    }

    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        foreach(var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
    }
}