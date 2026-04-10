using System;

namespace MonoGameLibrary.Components.Infrastructure
{
    interface IDependsOnOtherComponents
    {
        Type[] GetComponentDependencies { get; }
    }
}
