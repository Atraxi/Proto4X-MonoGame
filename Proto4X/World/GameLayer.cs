using Microsoft.Xna.Framework;
using Proto4X.Entities;
using System.Collections.Generic;

namespace Proto4x.World
{
    public class GameLayer
    {
        public readonly Dictionary<int, Entity> Entities;
        public readonly Rectangle Bounds;

        public GameLayer(int xPos, int yPos, int width, int height)
        {
            Entities = new();
            Bounds = new(xPos, yPos, width, height);
        }

        public void draw(Graphics graphics)
        {
            graphics.transform(Matrix.Translate(Bounds.X, Bounds.Y));

            Drawables.forEach(drawable => {
                drawable.value.draw(graphics, drawable.key);
            });
	    }
    }
}