using MonoGameLibrary.Components.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoGameLibrary.Archetypes
{
    public class ArchetypeChunk(Dictionary<Type, IComponentColumn> components, long entityCount)
    {
        public readonly ReadOnlyDictionary<Type, IComponentColumn> Components = components.AsReadOnly();

        public readonly long EntityCount = entityCount;

        public ref T Get<T>(int index) where T : struct
        {
            return ref ((ComponentColumn<T>)Components[typeof(T)]).Values[index];
        }
    }
}