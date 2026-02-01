using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Archetypes;
using System.Collections.Generic;

namespace MonoGameLibrary.Systems
{
    public interface IDrawSystem
    {
        void Draw(SpriteBatch spriteBatch, Rectangle viewport, List<ArchetypeBase> archetypes);
    }
}