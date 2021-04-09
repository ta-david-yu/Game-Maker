using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keep tracks of all the entity datas and their instances
/// </summary>
[CreateAssetMenu(fileName = "Entity Global Handler", menuName = "Global Handler/Entity Global Handler")]
public class EntityGlobalHandler : ScriptableObject
{
    /// <summary>
    /// Keep tracks of entity data and instance
    /// </summary>
    [System.Serializable]
    public class EntityEntry
    {
        public EntityData Data;
        public EntityInstance Instance;
    }

    public Transform EntityRoot { get; set; }

    public List<EntityEntry> EntityEntries = new List<EntityEntry>();

    /// <summary>
    /// Create an entity without any given data
    /// </summary>
    /// <param name="entitySO">The entity base SO</param>
    /// <returns></returns>
    public EntityEntry CreateEmptyEntity(EntitySO entitySO)
    {
        EntityData data = new EntityData() { EntitySO = entitySO, BehaviourDatas = new List<BehaviourData>() };

        // Create new entity instance
        // TODO: allocate from an object pool
        EntityInstance newInstance = Instantiate(data.EntitySO.EntityPrefab);

        // Register entry to the list
        EntityEntry entry = new EntityEntry() { Data = data, Instance = newInstance };
        EntityEntries.Add(entry);

        return entry;
    }

    /// <summary>
    /// Create a new entity from the given entity data
    /// </summary>
    /// <param name="data">The data used to create an entity</param>
    /// <returns></returns>
    public EntityEntry CreateEntity(EntityData data)
    {
        // Create new entity instance
        // TODO: allocate from an object pool
        EntityInstance newInstance = Instantiate(data.EntitySO.EntityPrefab);

        // Attach behaviours and setup parameters
        for (int i = 0; i < data.BehaviourDatas.Count; i++)
        {
            var behaviourData = data.BehaviourDatas[i];
            var behaviourInstance = behaviourData.BehaviourSO.AddBehaviourToEntity(newInstance);
            behaviourInstance.UpdateAllParameters(behaviourData.ParamDatas);
        }

        // Register entry to the list
        EntityEntry entry = new EntityEntry() { Data = data, Instance = newInstance };
        EntityEntries.Add(entry);

        return entry;
    }

    /// <summary>
    /// Remove an entity
    /// </summary>
    /// <param name="entry"></param>
    public void DeleteEntity(EntityEntry entry)
    {
        DeleteEntity(entry.Instance);
    }

    public void DeleteEntity(EntityInstance entityInstance)
    {
        // Deregister from the list
        for (int i = 0; i < EntityEntries.Count; i++)
        {
            var entry = EntityEntries[i];
            if (entry.Instance == entityInstance)
            {
                EntityEntries.RemoveAt(i);
                break;
            }
        }

        // Remove behaviour from the entity instance and the behaviourSO
        for (int i = 0; i < entityInstance.Behaviours.Count; i++)
        {
            var behaviourInstance = entityInstance.Behaviours[i];
            behaviourInstance.BehaviourSO.RemoveBehaviourFromEntity(entityInstance, behaviourInstance);
        }

        // Delete entity instance
        // TODO: recycle to an object pool
        Destroy(entityInstance.gameObject);
    }
}
