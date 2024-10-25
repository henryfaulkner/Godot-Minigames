using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumHelper
{
    public static string GetEnumDescription(Enum value)
    {
        // Get the type of the enum
        Type type = value.GetType();

        // Get the field info for the particular enum value
        FieldInfo fieldInfo = type.GetField(value.ToString());

        // Get the Description attribute, if present
        if (fieldInfo != null)
        {
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            if (attribute != null)
            {
                return attribute.Description;
            }
        }

        // Return the enum name if no description is found
        return value.ToString();
    }
}
