using Microsoft.Xna.Framework;
using MonoGameLibrary.Components;

namespace Proto4x.Components
{
    public struct Position(Vector2 location, float rotation = 0)
    {
        public Vector2 Location { get; set; } = location;
        public float Rotation { get; set; } = rotation;
    }

    public interface IPositionProvider : IComponentProvider {
        public Position[] Positions { get; }
    }
}
