using System.ComponentModel;
using System.Reflection;

namespace Summervik.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumerationValue) where T : struct, Enum
    {
        var type = enumerationValue.GetType();
        if (!type.IsEnum)
            throw new ArgumentException($"{nameof(T)} must be of type Enum.");

        bool hasFlagsAttribute = Attribute.IsDefined(type, typeof(FlagsAttribute));

        var memberInfo = type.GetMember(enumerationValue.ToString());

        if (hasFlagsAttribute)
        {
            List<string> results = new(10);
            foreach (T enumVal in Enum.GetValues(type).Cast<T>().Where(i => Convert.ToInt32(i) > 0))
            {
                if (enumerationValue.HasFlag(enumVal))
                {
                    var info = type.GetMember(enumVal.ToString());
                    if (info.Length > 0)
                    {
                        var attrs = info[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attrs.Length > 0)
                            results.Add(((DescriptionAttribute)attrs[0]).Description);
                        else
                            results.Add(enumVal.ToString());
                    }
                }
            }

            return string.Join(", ", results);
        }

        if (memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
            else
            {
                return enumerationValue.ToString();
            }
        }

        return default(T).ToString();
    }
}
