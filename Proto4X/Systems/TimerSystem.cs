using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components.Infrastructure;
using MonoGameLibrary.Systems;
using Proto4X.Components;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class TimerSystem : SystemBase, IUpdateSystem
    {
        public override ComponentTypeMask RequiredComponentProviders => ComponentTypeMask.FromTypes(
            typeof(Timer)
            );

        public void Update(GameTime gameTime, List<ArchetypeChunk> archetypeChunks)
        {
            var elapsedTime = gameTime.ElapsedGameTime;

            foreach (var archetypeChunk in archetypeChunks)
            {
                for (var entityIndex = 0; entityIndex < archetypeChunk.EntityCount; entityIndex++)
                {
                    ref Timer timer = ref archetypeChunk.Get<Timer>(entityIndex);

                    if (timer.HasJustElapsed)
                    {
                        timer.HasJustElapsed = false;
                    }
                    if (timer.Remaining > elapsedTime.Ticks)
                    {
                        timer.Remaining -= elapsedTime.Ticks;

                        if (timer.Remaining <= 0f)
                        {
                            timer.HasJustElapsed = true;

                            if (timer.ShouldRepeat)
                            {
                                timer.Remaining = timer.Duration;
                            }
                        }
                    }
                }
            }
        }
    }
}
