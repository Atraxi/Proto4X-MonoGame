using MonoGameLibrary.Components;
using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Archetypes
{
    public class ArchetypeChunk(Dictionary<Type, IComponentColumn> components, long entityCount)
    {
        private readonly Dictionary<Type, IComponentColumn> Components = components;

        public readonly long EntityCount = entityCount;

        public T[] Get<T>() where T : struct
        {
            return ((ComponentColumn<T>) Components[typeof(T)]).Values;
        }
    }
}