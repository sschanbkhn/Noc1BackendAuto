using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Helpers
{
    public static class EnumerationHelpers 
    {        
		public static List<Category> ToList<T>() where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("Type given T must be an Enum");
			}

			var enumType = typeof(T).Name;
			var valueDescriptions = System.Enum.GetValues(typeof(T))
				.Cast<System.Enum>()
				.Select(t => new Category { Id = (int)(object)t, Code = t.ToString(), Name = GetDescriptionFromEnum(t) }).ToList();

			return valueDescriptions;
		}
        public static T ParseEnum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }
        public static T GetEnumFromDescription<T>(string description) where T : System.Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
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
            // Or return default(T);
        }
        public static string GetDescriptionFromEnum(System.Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0)
				return attributes[0].Description;
			return value.ToString();
		}

		public static Type GetEnumType(string Namespace, string Name)
        {
            Type t = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load)
                    .SelectMany(x => x.DefinedTypes).
                    Where(o => o.IsEnum == true && o.Namespace == Namespace && o.Name == Name).FirstOrDefault();            
            return t;            
        }
        public static Type GetEnumType(string Namespace, string ClassName, string Name)
        {
            Type t = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load)
                    .SelectMany(x => x.DefinedTypes).
                    Where(o => o.IsEnum == true && o.Namespace == Namespace && o.ReflectedType.Name == ClassName && o.Name == Name).FirstOrDefault();
            return t;
        }
        public struct Category
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
