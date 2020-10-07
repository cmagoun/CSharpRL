using System;
using System.Collections.Generic;

namespace CsEcs
{
    public interface IComponent //: ISignalReceiver
    {
        string CName { get; }
        string CKey { get; }
        
        Type MyType { get; }
        string EntityId { get; set; }

        Ecs MyEcs { get; set; }
        IComponent Copy();

        //I am trying to avoid the need for these by
        //having no meaningful (game logic) behavior in the components
        void OnAdd();
        void OnDelete();
        //void OnEdit();
    }

    public abstract class Component<TEdit> : IComponent
    {
        public abstract Type MyType { get; }
        public string EntityId { get; set; }
        public string CName => MyType.Name;
        public string CKey => $"{EntityId}:{MyType.Name}";

        public Ecs MyEcs { get; set; }

        //When calling derivedObj.DoEdit(values) the line
        //base.DoEdit(values) needs to come AT THE TOP OF THE METHOD
        //so that the index will be edited properly with the old values
        public virtual void DoEdit(TEdit values)
        {
            if (this is IIndexable oldValues)
            {
                var newIndexable = (IIndexable)values;
                if(newIndexable.IndexKey != null) EditIndex(oldValues, newIndexable);
            }
        }

        public virtual void EditIndex(IIndexable oldValues, IIndexable newValues)
        {
            if (MyEcs.ComponentIndexes.ContainsKey(CName))
            {
                var index = MyEcs.ComponentIndexes[CName];
                if (index.ContainsKey(oldValues.IndexKey)) index[oldValues.IndexKey].Remove(EntityId);
                if (!index.ContainsKey(newValues.IndexKey)) index.Add(newValues.IndexKey, new List<string>());
                index[newValues.IndexKey].Add(EntityId);
            }
        }

        public virtual void OnAdd() { }
        public virtual void OnDelete() { }


        public abstract IComponent Copy();


    }
}
