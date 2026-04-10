using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Components
{
    public struct Position(Vector2 location, float rotation = 0)
    {
        public Vector2 Location = location;

        public float Rotation = rotation;
    }

    public struct VirtualPredictedPosition(Vector2 location, float rotation = 0)
    {
        public Vector2 Location = location;

        public float Rotation = rotation;

        public uint WillCollideWith = 0;

        public VirtualPredictedPosition(Position component) : this(component.Location, component.Rotation) {}
    }
}
