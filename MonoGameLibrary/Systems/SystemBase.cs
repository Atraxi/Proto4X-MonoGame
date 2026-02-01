using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Systems
{
    public abstract class SystemBase
    {
        public abstract IEnumerable<Type> RequiredComponentProviders { get; }
    }
}
