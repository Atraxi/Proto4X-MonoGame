using Microsoft.Xna.Framework;
using Proto4X.Archetypes;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public interface IUpdateSystem
    {
        void Update(GameTime gameTime, List<ArchetypeBase> archetypes);
    }
}