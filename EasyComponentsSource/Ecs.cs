using EasyComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsEcs
{
    public class Ecs
    {
        private Dictionary<string, Dictionary<string, IComponent>> EntitiesToComponents { get; }
        private Dictionary<string, Dictionary<string, bool>> ComponentsToEntities { get; }
      
        public Dictionary<string, Dictionary<string, List<string>>> ComponentIndexes { get; }
        public string Id { get; }
        public int EntityCount => EntitiesToComponents.Count;
        public int ComponentCount => ComponentsToEntities.Count;

        public Ecs(string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;

            EntitiesToComponents = new Dictionary<string, Dictionary<string, IComponent>>(); //entityid, component, data
            ComponentsToEntities = new Dictionary<string, Dictionary<string, bool>>(); //component, entityid, exists
            ComponentIndexes = new Dictionary<string, Dictionary<string, List<string>>>(); //component name, key, entityIds
        }

        public void AddComponent(string entityId, IComponent component)
        {
            if (!EntitiesToComponents.ContainsKey(entityId))
                throw new ArgumentException($"Entity {entityId} does not exist");

            ////What to do here... we can theoretically have two animation components on an item at once?
            if (EntitiesToComponents[entityId].ContainsKey(component.CName))
            {
                var oldComponent = EntitiesToComponents[entityId][component.CName];
                var mergable = oldComponent as IMergable;

                if (mergable != null)
                {
                    EntitiesToComponents[entityId][component.CName] = mergable.Merge(component);
                    return;
                } 
                else
                {
                    throw new ArgumentException($"Entity {entityId} already contains {component.CName}");
                }
            }

            component.MyEcs = this;
            EditComponent(entityId, component);
        }

        public void AddIndex(string indexId)
        {
            if(ComponentIndexes.ContainsKey(indexId)) throw new ArgumentException($"{indexId} already exists");
            ComponentIndexes.Add(indexId, new Dictionary<string, List<string>>());
        }

        public string CreateEntity(string id = "")
        {
            if(EntitiesToComponents.ContainsKey(id)) throw new Exception($"Entity {id} already exists");

            var myid = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            EntitiesToComponents.Add(myid, new Dictionary<string, IComponent>());
            return myid;
        }

        public string CopyEntityTo(string oldId, Ecs targetEcs, bool overwriteExisting = true, bool useExistingIds = true) 
        {
            if(overwriteExisting) targetEcs.DestroyEntity(oldId);

            try
            {
                var newId = useExistingIds
                    ? targetEcs.CreateEntity(oldId)
                    : targetEcs.CreateEntity();

                foreach (var comp in EntitiesToComponents[oldId].Values)
                {
                    targetEcs.AddComponent(newId, comp.Copy());
                }

                return newId;
            }
            catch
            {
                return "";
            }
        }

        public string MoveEntityTo(string oldId, Ecs targetEcs, bool overwriteExisting = true, bool useExistingIds = true)
        {
            if (overwriteExisting) targetEcs.DestroyEntity(oldId);

            var newId = useExistingIds
                ? targetEcs.CreateEntity(oldId)
                : targetEcs.CreateEntity();

            //this breaks because the comp references are the same as before
            foreach (var comp in EntitiesToComponents[oldId].Values)
            {
                targetEcs.AddComponent(newId, comp.Copy());
            }

            DestroyEntity(oldId);

            return newId;          
        }
        
        public void DestroyEntity(string entityId)
        {
            if (!EntitiesToComponents.ContainsKey(entityId)) return;

            //remove this entityid from all component lists
            foreach (var dc in ComponentsToEntities)
            {
                dc.Value.Remove(entityId);
            }
    
            //remove each component from the entity
            //to clean them all up
            var compNamesToRemove = new List<string>();
            foreach (var comp in EntitiesToComponents[entityId])
            {
                compNamesToRemove.Add(comp.Key);             
            }

            foreach (var name in compNamesToRemove)
            {
                RemoveComponent(entityId, name);
            }
            
            //remove from entity list
            EntitiesToComponents.Remove(entityId);
        }

        private void EditComponent(string entityId, IComponent component)
        {
            if(!EntitiesToComponents.ContainsKey(entityId)) throw new ArgumentException($"Entity {entityId} does not exist");

            component.EntityId = entityId;

            //The component is not on the entity in question, so add it
            if (!EntitiesToComponents[entityId].ContainsKey(component.CName))
            {
                EntitiesToComponents[entityId].Add(component.CName, component);

                if (!ComponentsToEntities.ContainsKey(component.CName)) ComponentsToEntities.Add(component.CName, new Dictionary<string, bool>());
                
                ComponentsToEntities[component.CName].Add(entityId, true);

                if (ComponentIndexes.ContainsKey(component.CName))
                {
                    //There is an index on this component (POSITION for instance)
                    var index = ComponentIndexes[component.CName];
                    var indexable = (IIndexable) component;
                    if (!index.ContainsKey(indexable.IndexKey)) index.Add(indexable.IndexKey, new List<string>());
                    index[indexable.IndexKey].Add(entityId);
                }
                component.OnAdd();

            } 
            else
            {            
                var oldComponent = EntitiesToComponents[entityId][component.CName];

 

                //remove the old component
                if (ComponentIndexes.ContainsKey(component.CName))
                {
                    var index = ComponentIndexes[component.CName];
                    var indexable = (IIndexable) oldComponent;
                    if (index.ContainsKey(indexable.IndexKey)) index[indexable.IndexKey].Remove(entityId);
                }

                oldComponent.OnDelete();

                //add the new component
                EntitiesToComponents[entityId][component.CName] = component;
                
                if (ComponentIndexes.ContainsKey(component.CName))
                {
                    var index = ComponentIndexes[component.CName];
                    var indexable = (IIndexable) component;
                    if(!index.ContainsKey(indexable.IndexKey)) index.Add(indexable.IndexKey, new List<string>());
                    index[indexable.IndexKey].Add(entityId);
                }
                component.OnAdd();
            }
        }

        public List<string> EntitiesInIndex(string componentName, string key)
        {
            if(!ComponentIndexes.ContainsKey(componentName)) return new List<string>();
            if(!ComponentIndexes[componentName].ContainsKey(key)) return new List<string>();
            return ComponentIndexes[componentName][key];
        }
        
        public List<string> EntitiesWith(params string[] componentNames)
        {
            var possibleEntities = componentNames
                .Select(cn => ComponentsToEntities.ContainsKey(cn) ? ComponentsToEntities[cn] : new Dictionary<string, bool>())
                .OrderBy(dc => dc.Count)
                .First();

            return possibleEntities.Where(pe =>
            {
                var dc = EntitiesToComponents[pe.Key];
                return componentNames.All(cn => dc.Keys.Contains(cn));
            })
                .Select(x => x.Key)
                .ToList();
        }

        public List<T> GetComponents<T>()
        {
            var entities = EntitiesWith(typeof(T).Name);
            return entities.Select(e => Get<T>(e)).ToList();
        }

        public List<T> GetComponents<T>(params string[] entityList)
        {
            var result = new List<T>();
            foreach (var entity in entityList)
            {
                var t = Get<T>(entity);
                if(t != null) result.Add(Get<T>(entity));
            }

            return result;
        }

        public List<Tuple<T,U>> GetComponents<T,U>()
        {
            var entities = EntitiesWith(typeof(T).Name, typeof(U).Name);
            return entities.Select(e => 
                new Tuple<T,U>(
                    Get<T>(e),
                    Get<U>(e)))
                .ToList();
        }

        public List<Tuple<T,U>> GetComponents<T, U>(params string[] entityList)
        {
            var result = new List<Tuple<T,U>>();
            foreach (var entity in entityList)
            {
                result.Add(new Tuple<T, U>(Get<T>(entity), Get<U>(entity)));
            }

            return result;
        }

        public List<Tuple<T,U,V>>GetComponents<T,U,V>()
        {
            var entities = EntitiesWith(typeof(T).Name, typeof(U).Name, typeof(V).Name);
            return entities.Select(e =>
                new Tuple<T, U, V>(
                    Get<T>(e),
                    Get<U>(e),
                    Get<V>(e)))
                .ToList();
        }

        public List<Tuple<T, U, V>> GetComponents<T, U, V>(params string[] entityList)
        {
            var result = new List<Tuple<T, U, V>>();
            foreach (var entity in entityList)
            {
                result.Add(new Tuple<T, U, V>(Get<T>(entity), Get<U>(entity), Get<V>(entity)));
            }

            return result;
        }

        public List<Tuple<T, U, V, W>>GetComponents<T,U,V,W>()
        {
            var entities = EntitiesWith(typeof(T).Name, typeof(U).Name, typeof(V).Name, typeof(W).Name);
            return entities.Select(e =>
                new Tuple<T, U, V, W>(
                    Get<T>(e),
                    Get<U>(e),
                    Get<V>(e),
                    Get<W>(e)))
                .ToList();
        }

        public List<Tuple<T, U, V, W>> GetComponents<T, U, V, W>(params string[] entityList)
        {
            var result = new List<Tuple<T, U, V, W>>();
            foreach (var entity in entityList)
            {
                result.Add(new Tuple<T, U, V, W>(Get<T>(entity), Get<U>(entity), Get<V>(entity), Get<W>(entity)));
            }

            return result;
        }

        public List<Tuple<T, U, V, W, X>> GetComponents<T, U, V, W, X>()
        {
            var entities = EntitiesWith(typeof(T).Name, typeof(U).Name, typeof(V).Name, typeof(W).Name, typeof(X).Name);
            return entities.Select(e =>
                new Tuple<T, U, V, W, X>(
                    Get<T>(e),
                    Get<U>(e),
                    Get<V>(e),
                    Get<W>(e),
                    Get<X>(e)))
                .ToList();
        }

        public List<Tuple<T, U, V, W, X>> GetComponents<T, U, V, W, X>(params string[] entityList)
        {
            var result = new List<Tuple<T, U, V, W, X>>();
            foreach (var entity in entityList)
            {
                result.Add(new Tuple<T, U, V, W, X>(Get<T>(entity), Get<U>(entity), Get<V>(entity), Get<W>(entity), Get<X>(entity)));
            }

            return result;
        }
     
        public Dictionary<string, IComponent> Entity(string id)
        {
            if (!EntitiesToComponents.ContainsKey(id)) return null;
            return EntitiesToComponents[id];
        }

        public T Get<T>(string entityId, string componentName = null)
        {
            var key = componentName ?? typeof(T).Name;

            if (!EntitiesToComponents[entityId].ContainsKey(key)) return default(T);

            return (T)EntitiesToComponents[entityId][key];
        }

        public Tuple<T, U> Get<T, U>(string entityId)
        {
            return new Tuple<T, U>(Get<T>(entityId), Get<U>(entityId));
        }

        public Tuple<T, U, V> Get<T, U, V>(string entityId)
        {
            return new Tuple<T, U, V>(Get<T>(entityId), Get<U>(entityId), Get<V>(entityId));
        }

        public Tuple<T, U, V, W> Get<T, U, V, W>(string entityId)
        {
            return new Tuple<T, U, V, W>(Get<T>(entityId), Get<U>(entityId), Get<V>(entityId), Get<W>(entityId));
        }

        public Tuple<T, U, V, W, X> Get<T, U, V, W, X>(string entityId)
        {
            return new Tuple<T, U, V, W, X>(Get<T>(entityId), Get<U>(entityId), Get<V>(entityId), Get<W>(entityId), Get<X>(entityId));
        }

        public List<IComponent> GetAll(string entityId)
        {
            return EntitiesToComponents[entityId].Values.ToList();
        }


        public bool Not<T>(string entity)
        {
            return Get<T>(entity) == null;
        }
        
        public EntityBuilder New(string id = "")
        {
            return EntityBuilder.New(this, id);
        }

        public void RemoveComponent(string entityId, string componentName)
        {
            if (!EntitiesToComponents.ContainsKey(entityId)) throw new ArgumentException($"Entity {entityId} does not exist");

            var comp = EntitiesToComponents[entityId][componentName];


            if (ComponentIndexes.ContainsKey(componentName))
            {
                var index = ComponentIndexes[componentName];
                var indexable = (IIndexable) comp;
                if (index.ContainsKey(indexable.IndexKey)) index[indexable.IndexKey].Remove(entityId);
            }

            comp.OnDelete();

            comp.MyEcs = null;    
            comp = null;

            EntitiesToComponents[entityId].Remove(componentName);
            ComponentsToEntities[componentName].Remove(entityId);

        }

        public void RemoveComponent(string entityId, IComponent component)
        {
            RemoveComponent(entityId, component.CName);
        }
    }

    public class EntityBuilder {
        private readonly string _id;
        private readonly Ecs _ecs;

        private EntityBuilder(Ecs ecs, string id = "")
        {
            _id = id;
            _ecs = ecs;
        }

        public static EntityBuilder New(Ecs ecs, string id = "")
        {
            var myid = ecs.CreateEntity(id);
            return new EntityBuilder(ecs, myid);
        }

        public EntityBuilder Add(IComponent component)
        {
            _ecs.AddComponent(_id, component);
            return this;
        }

        public EntityBuilder AddIf(bool condition, IComponent component)
        {
            if(condition) _ecs.AddComponent(_id, component);
            return this;
        }

        //"Completes" the entity and returns the id to the caller.
        //Useful if you need the ids and you are relying
        //on the ecs to create them
        public string Done()
        {
            return _id;
        }
    }

}
