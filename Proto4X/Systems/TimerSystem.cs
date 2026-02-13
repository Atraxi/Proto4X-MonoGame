using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Systems;
using Proto4X.Components;
using System;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class TimerSystem : SystemBase, IUpdateSystem
    {
        public override IEnumerable<Type> RequiredComponentProviders => [
            typeof(ITimerProvider)];

        public void Update(GameTime gameTime, List<ArchetypeBase> archetypes)
        {
            var elapsedTime = gameTime.ElapsedGameTime;
    
            foreach (var archetype in archetypes)
            {
                var timers = ((ITimerProvider)archetype).Timers;

                for (int i = 0; i < archetype.Count; i++)
                {
                    if (timers[i].HasJustElapsed)
                    {
                        timers[i].HasJustElapsed = false;
                    }
                    if (timers[i].Remaining > elapsedTime.Ticks)
                    {
                        timers[i].Remaining -= elapsedTime.Ticks;

                        if (timers[i].Remaining <= 0f)
                        {
                            timers[i].HasJustElapsed = true;

                            if (timers[i].ShouldRepeat)
                            {
                                timers[i].Remaining = timers[i].Duration;
                            }
                        }
                    }
                }
            }
        }
    }
}
