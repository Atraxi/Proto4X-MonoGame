using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using Proto4x.Components;
using System;

class PositionMotion(int initialCapacity) : ArchetypeBase<(Vector2 position, float radiansRotation, Vector2 velocity)>(initialCapacity), IPositionProvider, IMotionProvider
{
    public Position[] Positions => positions;
    private Position[] positions = new Position[initialCapacity];
    public Motion[] MotionValues => motionValues;
    private Motion[] motionValues = new Motion[initialCapacity];

    public override void OnAddEntity(int index, (Vector2 position, float radiansRotation, Vector2 velocity) payload)
    {
        Positions[index] = new(payload.position, payload.radiansRotation);
        MotionValues[index] = new(payload.velocity);
    }

    public override void OnRemoveEntity(int index)
    {
        int last = Count;

        Positions[index] = Positions[last];
        MotionValues[index] = MotionValues[last];
    }

    protected override void Grow(int newSize)
    {
        Array.Resize(ref positions, newSize);
        Array.Resize(ref motionValues, newSize);
    }
}