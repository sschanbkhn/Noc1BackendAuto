using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class ColumnNameAttr : Attribute
    {
        public readonly string Column;

        public ColumnNameAttr(string columnName)
        {
            this.Column = columnName;
        }
    }
}
