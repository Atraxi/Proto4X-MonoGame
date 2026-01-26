using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto4x.World;
using Proto4X.Archetypes;
using Proto4X.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto4X.Systems
{
    public class SystemScheduler
    {
        private readonly Dictionary<GameLayer, List<IUpdateSystem>> _updateSystems = [];
        private readonly Dictionary<GameLayer, List<IDrawSystem>> _drawSystems = [];
        private readonly Dictionary<GameLayer, List<SystemBase>> _allSystems = [];

        public void Add<T>(GameLayer gameLayer, T system) where T : SystemBase
        {
            _allSystems.AddToInternalList(gameLayer, system);

            if (system is IUpdateSystem update)
            {
                _updateSystems.AddToInternalList(gameLayer, update);
            }

            if (system is IDrawSystem draw)
            {
                _drawSystems.AddToInternalList(gameLayer, draw);
            }
        }

        public void Update(GameLayer gameLayer, GameTime gameTime)
        {
            foreach (var system in _updateSystems[gameLayer]) {
                var systemAsBase = (SystemBase)system;//Guaranteed because we use add<T>(...) where T : SystemBase, would use a union type if they were available

                //TODO maybe we can cache these? type lookups every frame/update are not cheap but not brutal
                var archetypes = gameLayer.QueryRelevantArchetypes(systemAsBase.RequiredComponentProviders);
                system.Update(gameTime, archetypes);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle viewport, GameLayer gameLayer)
        {
            foreach (var system in _drawSystems[gameLayer])
            {
                var systemAsBase = (SystemBase)system;//Guaranteed because we use add<T>(...) where T : SystemBase, would use a union type if they were available
                
                //TODO maybe we can cache these? type lookups every frame/update are not cheap but not brutal
                var archetypes = gameLayer.QueryRelevantArchetypes(systemAsBase.RequiredComponentProviders);
                system.Draw(spriteBatch, viewport, archetypes);
            }
        }
    }
}
