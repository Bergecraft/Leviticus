using Assets.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Entities.Modules
{
    public interface IContainerBehaviour<T> where T : ItemDef
    {
        void Add(T item);
        void Remove(T item);
    }
}
