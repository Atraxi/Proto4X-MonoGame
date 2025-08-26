using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto4x.Components
{

    internal class Components<T>
    {
        private readonly Dictionary<int, T> _components = new();

        public void Add(int id, T component)
        {
            _components[id] = component;
        }

        public IReadOnlyDictionary<int, T> GetAll()
        {
            return _components;
        }

        public void Remove(int id)
        {
            _components.Remove(id);
        }
    }
}
