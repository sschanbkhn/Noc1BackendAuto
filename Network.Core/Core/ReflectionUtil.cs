using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Core
{
    public class ReflectionUtil
    {
        public static List<string> GetColumnNameAttr<T>(string columnCondition)
        {
            List<string> _list = new List<string>();

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ColumnNameAttr columnNameAttr = attr as ColumnNameAttr;
                    if (columnNameAttr != null)
                    {
                        string propName = prop.Name;
                        string column = columnNameAttr.Column;
                        if(string.IsNullOrEmpty(columnCondition))
                            _list.Add(propName);
                        else if(column == columnCondition)
                            _list.Add(propName);
                    }
                }
            }

            return _list;
        }
        public static IEnumerable<string> GetControllers(string assemblyFile)
        {
            Assembly asm = Assembly.LoadFrom(assemblyFile);
            var controllers = asm.GetTypes()
                .Where(o => typeof(ControllerBase).IsAssignableFrom(o.BaseType) && o.Name.Contains("Controller") && !o.IsAbstract)
                .Select(o => o.Name);            
            return controllers;
        }
        public static IEnumerable<string> GetActionsWithController(string assemblyFile)
        {
            Assembly asm = Assembly.LoadFrom(assemblyFile);
            var actions = asm.GetTypes()
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                .Where(action => action.DeclaringType.ToString().Contains("Controllers.")
                && !action.ReflectedType.Name.Contains("ApiControllerBase"))
                .Select(o => o.ReflectedType.Name + "." + o.Name);
            return actions;        
        }
    }
}
