using MonoGameLibrary.Components;

namespace MonoGameLibrary.Systems
{
    public abstract class SystemBase
    {
        public abstract ComponentTypeMask RequiredComponentProviders { get; }
    }
}
