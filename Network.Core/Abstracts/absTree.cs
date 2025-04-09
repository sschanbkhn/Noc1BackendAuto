using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Abstracts
{
    public abstract class absTree<TParent>
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int NodeLevel { get; set; }
        public string ParentId { get; set; }
        public List<TParent> Children { get; set; } = new List<TParent>();
    }
}
