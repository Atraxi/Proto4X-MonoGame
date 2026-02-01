using MonoGameLibrary.Components;

namespace Proto4X.Components
{
    public enum Faction
    {
        Player,
        Enemy,
    }
    public interface IFactionProvider : IComponentProvider
    {
        public Faction[] FactionValues { get; }
    }
}
