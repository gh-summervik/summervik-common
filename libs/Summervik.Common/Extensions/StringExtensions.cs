using System.ComponentModel;
using System.Reflection;

namespace Summervik.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts a string to an enum value.
    /// The conversion uses the <see cref="DescriptionAttribute"/> value if
    /// has been applied to the enum value.
    /// </summary>
    public static T ToEnum<T>(this string text) where T : struct, Enum
    {
        var type = typeof(T);
        if (!type.IsEnum)
            throw new ArgumentException($"{nameof(T)} must be of type Enum.");

        if (text.Contains(','))
        {
            List<string> strValues = new(10);
            string[] split = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var str in split)
            {
                MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
                foreach (MemberInfo member in members)
                {
                    var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        for (int i = 0; i < attrs.Length; i++)
                        {
                            string description = ((DescriptionAttribute)attrs[i]).Description;
                            if (str.Equals(description, StringComparison.OrdinalIgnoreCase))
                                strValues.Add(((T)Enum.Parse(type, member.Name, true)).ToString());
                        }
                    }
                    else if (member.Name.Equals(str, StringComparison.OrdinalIgnoreCase))
                        strValues.Add(((T)Enum.Parse(type, member.Name, true)).ToString());
                }
            }

            return (T)Enum.Parse(type, string.Join(", ", strValues));
        }
        else
        {
            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (MemberInfo member in members)
            {
                var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        string description = ((DescriptionAttribute)attrs[i]).Description;
                        if (text.Equals(description, StringComparison.OrdinalIgnoreCase))
                            return (T)Enum.Parse(type, member.Name, true);
                    }
                }
                if (member.Name.Equals(text, StringComparison.OrdinalIgnoreCase))
                    return (T)Enum.Parse(type, member.Name, true);
            }

            return default;
        }

        throw new ArgumentException($"No enum value found for description '{text}'.", nameof(text));
    }
}
