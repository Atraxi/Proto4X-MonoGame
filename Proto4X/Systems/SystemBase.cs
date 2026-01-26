using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto4X.Systems
{
    public abstract class SystemBase
    {
        public abstract IEnumerable<Type> RequiredComponentProviders { get; }
    }
}
