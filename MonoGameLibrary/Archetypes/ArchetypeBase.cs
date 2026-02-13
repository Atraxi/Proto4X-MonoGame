using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Archetypes
{
    public abstract class ArchetypeBase<TPayload>(int initialCapacity) : ArchetypeBase(initialCapacity)
    {
        private List<int> DeferredEntityRemovalsByIndex = [];
        private List<(int entityId, TPayload payload)> DeferredAdditions = [];

        public void AddEntity(int entityId, TPayload payload)
        {
            int index = count++;

            if (index == DenseToEntity.Length)
            {
                Grow();
            }

            DenseToEntity[index] = entityId;
            EntityToDense[entityId] = index;

            OnAddEntity(index, payload);
        }
        protected abstract void OnAddEntity(int entityId, TPayload payload);

        public void RemoveEntity(int entityId)
        {
            int index = EntityToDense[entityId];
            int last = --count;
    
            if (index != last)
            {
                int movedEntity = DenseToEntity[last];
                DenseToEntity[index] = movedEntity;
                EntityToDense[movedEntity] = index;
            }

            OnRemoveEntity(index);
        }
        protected abstract void OnRemoveEntity(int entityId);

        public void QueueEntityForAddition(int entityId, TPayload entityData)
        {
            DeferredAdditions.Add((entityId, entityData));
        }

        public void QueueEntityForRemovalById(int entityId)
        {
            DeferredEntityRemovalsByIndex.Add(EntityToDense[entityId]);
        }

        public void QueueEntityForRemovalByIndex(int entityIndex)
        {
            DeferredEntityRemovalsByIndex.Add(entityIndex);
        }

        internal override void ProcessDeferredUpdates()
        {
            foreach ((int entityId, TPayload payload) in DeferredAdditions)
            {
                AddEntity(entityId, payload);
            }

            DeferredEntityRemovalsByIndex.Sort((int A, int B) => B - A);
            foreach (var  entityId in DeferredEntityRemovalsByIndex)
            {
                RemoveEntity(entityId);
            }
        }
    }

    public abstract class ArchetypeBase(int initialCapacity)
    {
        protected int[] DenseToEntity = new int[initialCapacity];
        protected int[] EntityToDense = new int[initialCapacity];

        public int Count => count;
        protected int count;

        public ReadOnlySpan<int> EntityIds => entityIds;
        private int[] entityIds = new int[initialCapacity];

        protected void Grow()
        {
            int newSize = DenseToEntity.Length * 2;

            Array.Resize(ref DenseToEntity, newSize);
            Array.Resize(ref EntityToDense, newSize);
            Array.Resize(ref entityIds, newSize);

            Grow(newSize);
        }

        protected abstract void Grow(int newSize);

        internal abstract void ProcessDeferredUpdates();
    }
}
