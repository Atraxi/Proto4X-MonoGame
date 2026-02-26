using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Utils;
using MonoGameLibrary.World;
using System.Collections.Generic;

namespace MonoGameLibrary.Systems
{
    public class SystemScheduler
    {
        private readonly Dictionary<GameWorld, List<IUpdateSystem>> _updateSystems = [];
        private readonly Dictionary<GameWorld, List<IDrawSystem>> _drawSystems = [];

        public void Add<T>(GameWorld gameLayer, T system) where T : SystemBase
        {
            if (system is IUpdateSystem update)
            {
                _updateSystems.AddToInternalList(gameLayer, update);
            }

            if (system is IDrawSystem draw)
            {
                _drawSystems.AddToInternalList(gameLayer, draw);
            }
        }

        public void Update(GameWorld gameLayer, GameTime gameTime)
        {
            foreach (var system in _updateSystems[gameLayer])
            {
                var systemAsBase = (SystemBase)system;//Guaranteed because we use Add<T>(...) where T : SystemBase, would use a union type if they were available

                //TODO maybe we can cache these? type lookups every frame/update are not cheap but not brutal
                var archetypeChunks = gameLayer.QueryRelevantComponentArrays(systemAsBase.RequiredComponentProviders);
                if (archetypeChunks.Count > 0)
                {
                    system.Update(gameTime, archetypeChunks);
                }
            }

            foreach (var archetypesKeyPair in gameLayer.Archetypes)
            {
                archetypesKeyPair.Value.ProcessDeferredUpdates();
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle viewport, GameWorld gameLayer)
        {
            spriteBatch.Begin();
            foreach (var system in _drawSystems[gameLayer])
            {
                var systemAsBase = (SystemBase)system;//Guaranteed because we use add<T>(...) where T : SystemBase, would use a union type if they were available
                
                //TODO maybe we can cache these? type lookups every frame/update are not cheap but not brutal
                var archetypeChunks = gameLayer.QueryRelevantComponentArrays(systemAsBase.RequiredComponentProviders);
                if (archetypeChunks.Count > 0)
                {
                    system.Draw(spriteBatch, viewport, archetypeChunks);
                }
            }
            spriteBatch.End();
        }
    }
}
