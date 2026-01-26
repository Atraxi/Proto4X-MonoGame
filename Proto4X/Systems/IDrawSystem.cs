using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto4X.Archetypes;
using System;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public interface IDrawSystem
    {
        void Draw(SpriteBatch spriteBatch, Rectangle viewport, List<ArchetypeBase> archetypes);
    }
}