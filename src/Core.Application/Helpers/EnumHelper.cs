using Core.Application.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Helpers
{
    public static class EnumHelper
    {
        public static List<VmSelectList> GetEnumList<T>() where T : struct // get enum list with discription
        {
            var list = new List<VmSelectList>();
            var counter = 1;
            foreach (int e in Enum.GetValues(typeof(T)))
            {
                var d = new VmSelectList();
                d.Id = e;
                d.Name = GetEnumDescription<T>(counter);
                list.Add(d);
                counter++;
            }
            return list;
        }

        private static string GetEnumDescription<T>(int counter)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new ArgumentException("Only Enum types allowed");
            var result = "";
            var c = 1;
            foreach (var value in Enum.GetValues(type).Cast<Enum>())
            {
                result =  getEnumDescription(value);
                if (c == counter)
                    break;
                c++;
            }
            return result;
        }

        public static string getEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }
}
