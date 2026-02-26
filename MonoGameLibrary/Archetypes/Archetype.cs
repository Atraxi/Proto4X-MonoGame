using MonoGameLibrary.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameLibrary.Archetypes
{
    public class Archetype
    {
        protected int[] DenseIndexToEntityId;
        protected Dictionary<int, int> EntityIdToDenseIndex = [];

        public readonly ComponentTypeMask ComponentMask;

        public int Count => count;
        private int count;

        public ReadOnlySpan<int> EntityIds => entityIds;
        private int[] entityIds;

        private readonly List<int> DeferredEntityRemovalsByIndex = [];
        private readonly List<EntityBuilder> DeferredAdditions = [];

        private readonly Dictionary<int, int> componentTypeIdToComponentArrayIndex = [];
        private IComponentColumn[] componentArrays;

        public Archetype(int initialCapacity, EntityBuilder entityBuilder)
        {
            DenseIndexToEntityId = new int[initialCapacity];
            ComponentMask = entityBuilder.ComponentTypeMask;
            entityIds = new int[initialCapacity];
            
            componentArrays = new IComponentColumn[entityBuilder.Components.Count];
            for (int componentsIndex = 0; componentsIndex < entityBuilder.Components.Count; componentsIndex++)
            {
                entityBuilder.Components[componentsIndex].CreateComponentColumn(this, componentsIndex, initialCapacity);
            }

            AddEntity(entityBuilder);
        }

        internal void CreateComponentColumn<T>(int componentsIndex, int initialCapacity) where T : struct
        {
            componentTypeIdToComponentArrayIndex[ComponentTypeRegistry.GetId(typeof(T))] = componentsIndex;
            componentArrays[componentsIndex] = new ComponentColumn<T>(new T[initialCapacity]);
        }

        public void AddEntity(EntityBuilder componentPayloads)
        {
            int entityId = EntityIdRegistry.GetNextFreeId();
            int entityIndex = count++;

            if (entityIndex == DenseIndexToEntityId.Length)
            {
                GrowEntityCapacity();
            }

            DenseIndexToEntityId[entityIndex] = entityId;
            EntityIdToDenseIndex[entityId] = entityIndex;

            foreach (var payload in componentPayloads.Components)
            {
                payload.WriteComponent(this, entityIndex);
            }
        }

        /// <summary>
        /// This method exists to allow an entity to provide an arbitrary number of components, while also getting around a generic type erasure boundary eforced by the necessity of said array of components of arbitrary value types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentData"></param>
        internal void WriteComponent<T>(T componentData, int entityIndex) where T : struct
        {
            var componentTypeId = ComponentTypeRegistry.GetId(componentData.GetType());
            int componentsIndex = componentTypeIdToComponentArrayIndex[componentTypeId];
            
            //We are able to assume the capacity will be sufficient due to a check and grow being performed above in AddEntity(...)
            //I'd like to remove the cast if possible. It should be 100% safe though
            ((ComponentColumn<T>)componentArrays[componentsIndex]).Values[entityIndex] = componentData;
        }

        internal ArchetypeChunk GetMatchingComponents(ComponentTypeMask queriedTypesMask)
        {
            return new ArchetypeChunk(componentArrays.Where(componentColumn => queriedTypesMask.ContainsType(componentColumn.GetComponentType()))
                    .ToDictionary(componentColumn => componentColumn.GetComponentType()),
                count);
        }

        public void RemoveEntity(int entityId)
        {
            int index = EntityIdToDenseIndex[entityId];
            int last = --count;
    
            if (index != last)
            {
                int movedEntity = DenseIndexToEntityId[last];
                DenseIndexToEntityId[index] = movedEntity;
                EntityIdToDenseIndex[movedEntity] = index;
            }
            foreach (var componentData in componentArrays)
            {
                componentData.RemoveAtIndexAndCopyLast(index, last);
            }
        }

        public void QueueEntityForAddition(EntityBuilder entityData)
        {
            DeferredAdditions.Add(entityData);
        }

        public void QueueEntityForRemovalById(int entityId)
        {
            DeferredEntityRemovalsByIndex.Add(EntityIdToDenseIndex[entityId]);
        }

        public void QueueEntityForRemovalByIndex(int entityIndex)
        {
            DeferredEntityRemovalsByIndex.Add(entityIndex);
        }

        internal void ProcessDeferredUpdates()
        {
            foreach (EntityBuilder payload in DeferredAdditions)
            {
                AddEntity(payload);
            }
            DeferredAdditions.Clear();

            DeferredEntityRemovalsByIndex.Sort((A, B) => B - A);
            foreach (var  entityId in DeferredEntityRemovalsByIndex)
            {
                RemoveEntity(entityId);
            }
            DeferredEntityRemovalsByIndex.Clear();
        }

        protected void GrowEntityCapacity()
        {
            int newSize = DenseIndexToEntityId.Length * 2;

            Array.Resize(ref DenseIndexToEntityId, newSize);
            Array.Resize(ref entityIds, newSize);

            foreach(var componentColumn in componentArrays)
            {
                componentColumn.Grow(newSize);
            }
        }

        protected void GrowComponentCapacity()
        {
            int newSize = componentArrays.Length * 2;

            Array.Resize(ref componentArrays, newSize);
        }
    }
}
