using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
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
                var timers = archetypeChunk.Get<Timer>();

                for (var entityIndex = 0; entityIndex < timers.Length; entityIndex++)
                {
                    ref Timer timer = ref timers[entityIndex];
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
