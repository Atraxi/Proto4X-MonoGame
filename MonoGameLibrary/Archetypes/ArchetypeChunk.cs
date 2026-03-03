using MonoGameLibrary.Components.Infrastructure;
using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Archetypes
{
    public class ArchetypeChunk(Dictionary<Type, IComponentColumn> components, long entityCount)
    {
        private readonly Dictionary<Type, IComponentColumn> Components = components;

        public readonly long EntityCount = entityCount;

        public ref T Get<T>(int index) where T : struct
        {
            return ref ((ComponentColumn<T>)Components[typeof(T)]).Values[index];
        }
    }
}