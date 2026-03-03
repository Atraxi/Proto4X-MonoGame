using MonoGameLibrary.Components.Infrastructure;

namespace MonoGameLibrary.Systems
{
    public abstract class SystemBase
    {
        public abstract ComponentTypeMask RequiredComponentProviders { get; }
    }
}
