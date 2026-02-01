using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using System.Collections.Generic;

namespace MonoGameLibrary.Systems
{
    public interface IUpdateSystem
    {
        void Update(GameTime gameTime, List<ArchetypeBase> archetypes);
    }
}