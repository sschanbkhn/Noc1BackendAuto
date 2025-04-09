using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Network.Core.Helpers
{
    public static class ObjectHelpers
    {
        public static string AsJsonString(this object obj)
        {
            var content = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return content;
        }
        public static dynamic ToDynamic<T>(this T obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var currentValue = propertyInfo.GetValue(obj);
                expando.Add(propertyInfo.Name, currentValue);
            }
            return expando as ExpandoObject;
        }        
        public static void AddProperty(ref ExpandoObject expando, string key, string value) 
        {
            ((IDictionary<string, object>)expando)[key] = value;            
        }
        public static object GetValue(this ExpandoObject expando, string name)
        {
            var expandoDic = (IDictionary<string, object>)expando;
            return expandoDic.ContainsKey(name) ? expandoDic[name] : null;
        }
        public static void Mapping<TFrom, TTo>(TFrom From, TTo To, string[] ignoreProperties = null)
        {
            List<PropertyInfo> sources = From.GetType().GetProperties().ToList();
            List<PropertyInfo> destinations = To.GetType().GetProperties().ToList();
            foreach (PropertyInfo source in sources)
            {                
                if ((!source.Name.ToLower().Equals("Cx".ToLower()) && !source.Name.ToLower().Equals("ClusteredIndex".ToLower())) && source.PropertyType.IsPublic)
                    if (source.PropertyType == typeof(Guid) ||
                        source.PropertyType == typeof(Guid?) ||
                        source.PropertyType == typeof(string) ||
                        source.PropertyType == typeof(int) ||
                        source.PropertyType == typeof(int?) ||
                        source.PropertyType == typeof(long) ||
                        source.PropertyType == typeof(long?) ||
                        source.PropertyType == typeof(byte) ||
                        source.PropertyType == typeof(byte[]) ||
                        source.PropertyType == typeof(byte?) ||
                        source.PropertyType == typeof(double) ||
                        source.PropertyType == typeof(double?) ||
                        source.PropertyType == typeof(decimal) ||
                        source.PropertyType == typeof(decimal?) ||
                        source.PropertyType == typeof(DateTime) ||
                        source.PropertyType == typeof(DateTime?) ||
                        source.PropertyType == typeof(Array) ||
                        source.PropertyType == typeof(bool) ||
                        source.PropertyType == typeof(bool?))
                    {
                        PropertyInfo destination = destinations.Where(d => d.Name.Equals(source.Name)).FirstOrDefault();
                        if (destination != null && destination.CanWrite && (
                            destination.PropertyType == typeof(Guid) ||
                            destination.PropertyType == typeof(Guid?) ||
                            destination.PropertyType == typeof(string) ||
                            destination.PropertyType == typeof(int) ||
                            destination.PropertyType == typeof(int?) ||
                            destination.PropertyType == typeof(long) ||
                            destination.PropertyType == typeof(long?) ||
                            destination.PropertyType == typeof(byte) ||
                            destination.PropertyType == typeof(byte[]) ||
                            destination.PropertyType == typeof(byte?) ||
                            destination.PropertyType == typeof(double) ||
                            destination.PropertyType == typeof(double?) ||
                            destination.PropertyType == typeof(decimal) ||
                            destination.PropertyType == typeof(decimal?) ||
                            destination.PropertyType == typeof(DateTime) ||
                            destination.PropertyType == typeof(DateTime?) ||
                            destination.PropertyType == typeof(Array) ||
                            destination.PropertyType == typeof(bool) ||
                            destination.PropertyType == typeof(bool?)
                        ))
                        {
                            bool ignore = false;
                            if (ignoreProperties != null)
                            {
                                foreach (var propName in ignoreProperties)
                                {
                                    if (source.Name.ToLower().Contains(propName.ToLower()))
                                    {
                                        ignore = true;
                                        break;
                                    }
                                }
                                destination.SetValue(To, destination.GetValue(To));
                            }    
                            if(ignore)
                            {
                                destination.SetValue(To, destination.GetValue(To));
                            } 
                            else
                            {
                                destination.SetValue(To, source.GetValue(From));
                            }    
                        }
                    }
            }
        }
        public static void Mapping<TFrom, TTo>(List<TFrom> lFrom, List<TTo> lTo)
        {
            for (int i = 0;i < lFrom.Count;i++)
            {
                if(lTo.ElementAtOrDefault(i) != null)
                {
                    Mapping<TFrom, TTo>(lFrom[i], lTo[i]);
                }    
                else
                {
                    TTo to = (TTo)Activator.CreateInstance(typeof(TTo));
                    Mapping<TFrom, TTo>(lFrom[i], to);
                    lTo.Add(to);
                }    
            }    
        }
    }
}
