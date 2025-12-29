using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto4x.Components;
using Proto4X.Components;
using Proto4X.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Proto4x.World
{
    public class GameLayer(int xPos, int yPos, int width, int height)
    {
        public readonly Dictionary<int, Entity> Entities = [];
        public readonly Rectangle Bounds = new(xPos, yPos, width, height);
        private readonly ComponentStore<Drawable> Drawables = new();
        private readonly ComponentStore<Position> EntityPositions = new();

        public void Draw(SpriteBatch spriteBatch, Rectangle viewport)
        {
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(Bounds.X, Bounds.Y, 0/*I think? 2d game not 3d. Confirm*/));

            foreach(var drawable in Drawables.GetAll().Values.Where(drawable => drawable.GetBounds().Intersects(viewport))) {
                drawable.Draw(spriteBatch, EntityPositions.Get(drawable.Id));
            }

            spriteBatch.End();
	    }
    }
}