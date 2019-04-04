using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace MedicalExaminer.Common.Enums
{
    /// <summary>
    /// Enum Descriptions Extension.
    /// </summary>
    public static class EnumDescriptionsExtension
    {
        /// <summary>
        /// Get Description.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <param name="e">Enum.</param>
        /// <returns>Description.</returns>
        public static string GetDescription<T>(this T e)
            where T : IConvertible
        {
            if (e is Enum)
            {
                var type = e.GetType();
                var values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));

                        if (memInfo
                            .First()
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null;
        }
    }
}
