using System;

namespace MonoGameLibrary.Components.Infrastructure
{
    public interface IComponentColumn
    {
        internal void Grow(int newSize);
        internal void RemoveAtIndexAndCopyLast(int removalIndex, int lastIndex);
        Type GetComponentType();
        IComponentContainer GetComponentContainerForEntity(int entityIndex, Archetypes.Archetype archetype);
    }

    public class ComponentColumn<T>(T[] values) : IComponentColumn where T: struct
    {
        public T[] Values => values;

        public IComponentContainer GetComponentContainerForEntity(int entityIndex, Archetypes.Archetype archetype)
            => new ComponentContainerAttached<T>(values[entityIndex], archetype);

        public Type GetComponentType()
            => typeof(T);

        public void Grow(int newSize)
            => Array.Resize(ref values, newSize);

        public void RemoveAtIndexAndCopyLast(int removalIndex, int lastIndex)
            => values[removalIndex] = values[lastIndex];
    }
}
