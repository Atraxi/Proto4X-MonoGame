using Microsoft.Xna.Framework;
using Proto4x.World;
using Proto4X.Components;
using System;

namespace Proto4x.Components
{
    public struct Velocity(Vector2 velocity)
    {

    }

    public interface IVelocityProvider : IComponentProvider {
        public Velocity[] Velocities { get; }
    }
}
