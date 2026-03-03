using Microsoft.Xna.Framework;

namespace Proto4X.Components
{
    public struct Position(Vector2 location, float rotation = 0)
    {
        public Vector2 Location = location;

        public float Rotation = rotation;
    }
}
