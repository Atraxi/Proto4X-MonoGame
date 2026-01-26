using Microsoft.Xna.Framework;
using Proto4x.Components;
using Proto4X.Archetypes;
using System;

class PositionVelocity(int initialCapacity) : ArchetypeBase<(Vector2 position, float radiansRotation, Vector2 velocity)>(initialCapacity), IPositionProvider, IVelocityProvider
{
    public Position[] Positions => positions;
    private Position[] positions = new Position[initialCapacity];
    public Velocity[] Velocities => velocities;
    private Velocity[] velocities = new Velocity[initialCapacity];

    public override void OnAddEntity(int index, (Vector2 position, float radiansRotation, Vector2 velocity) payload)
    {
        Positions[index] = new(payload.position, payload.radiansRotation);
        Velocities[index] = new(payload.velocity);
    }

    public override void OnRemoveEntity(int index)
    {
        int last = Count;

        Positions[index] = Positions[last];
        Velocities[index] = Velocities[last];
    }

    protected override void Grow(int newSize)
    {
        Array.Resize(ref positions, newSize);
        Array.Resize(ref velocities, newSize);
    }
}