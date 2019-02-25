using System;
using System.Collections.Generic;
using System.Linq;
using Medical_Examiner_API.Enums;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Models.V1.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Medical_Examiner_API.Extensions.Data
{
    public static class EnumExtentions
    {
        /// <summary>
        /// Convert a <see cref="MeUser"/>  to a <see cref="UserItem"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A UserItem.</returns>
        public static IDictionary<string, string> GetDictionary(this Type enumType)
        {
            if (enumType.IsEnum)
            {
                Dictionary<string, string> enum_dictionary = new Dictionary<string, string>();

                foreach (var value in Enum.GetValues(enumType))
                {
                    var name = enumType.GetEnumName(value);
                    enum_dictionary.Add(name, (string)value);
                }

                return enum_dictionary;
            }

            return null;
        }
    }
}