using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Keep tracks of all the entity datas and their instances
/// </summary>
[CreateAssetMenu(fileName = "Entity Global Handler", menuName = "Global Handler/Entity Global Handler")]
public class EntityGlobalHandler : ScriptableObject, ISerializationCallbackReceiver
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

    public int EntityIDCounter { get; private set; } = 0;

    public Transform EntityRoot { get; set; }

    [System.NonSerialized]
    private List<EntityEntry> m_EntityEntries = new List<EntityEntry>();
    public ReadOnlyCollection<EntityEntry> EntityEntries { get { return m_EntityEntries.AsReadOnly(); } }

    [System.NonSerialized]
    private Dictionary<int, EntityEntry> m_EntityEntryLookUpTable = new Dictionary<int, EntityEntry>();

    public void OnAfterDeserialize()
    {
        EntityEntries = new List<EntityEntry>();
        m_EntityEntryLookUpTable = new Dictionary<int, EntityEntry>();
    }

    public void OnBeforeSerialize()
    {
    }

    public EntityEntry GetEntityEntry(int entityID)
    {
        if (m_EntityEntryLookUpTable.ContainsKey(entityID))
        {
            return m_EntityEntryLookUpTable[entityID];
        }
        return null;
    }

    /// <summary>
    /// Create a new level, delete all the entities in the scene
    /// </summary>
    public void CreateNewScene()
    {
        for (int i = 0; i < EntityEntries.Count; i++)
        {
            var entityInstance = EntityEntries[i].Instance;

            // Remove behaviour from the entity instance and the behaviourSO
            for (int j = 0; j < entityInstance.Behaviours.Count; j++)
            {
                var behaviourInstance = entityInstance.Behaviours[i];
                behaviourInstance.BehaviourSO.RemoveBehaviourFromEntity(entityInstance, behaviourInstance);
            }

            // Delete entity instance
            // TODO: recycle to an object pool
            Destroy(entityInstance.gameObject);
        }

        m_EntityEntries = new List<EntityEntry>();
        m_EntityEntryLookUpTable = new Dictionary<int, EntityEntry>();
    }

    /// <summary>
    /// Create an entity without any given data
    /// </summary>
    /// <param name="entitySO">The entity base SO</param>
    /// <returns></returns>
    public EntityEntry CreateEmptyEntity(EntitySO entitySO)
    {
        int id = EntityIDCounter;

        EntityData data = new EntityData() { 
            EntityName = entitySO.EntityDefaultName, 
            EntitySO = entitySO, 
            BehaviourDatas = new List<BehaviourData>() };

        // Create new entity instance
        EntityInstance newInstance = Instantiate(data.EntitySO.EntityPrefab, EntityRoot);
        newInstance.gameObject.name = data.EntityName;

        // Register entry to the list
        EntityEntry entry = new EntityEntry() { Data = data, Instance = newInstance };
        m_EntityEntries.Add(entry);
        m_EntityEntryLookUpTable.Add(id, entry);
        newInstance.OnInstantiate(id);

        EntityIDCounter++;

        return entry;
    }

    /// <summary>
    /// Create a new entity from the given entity data
    /// </summary>
    /// <param name="data">The data used to create an entity</param>
    /// <returns></returns>
    public EntityEntry CreateEntity(EntityData data)
    {
        int id = EntityIDCounter;

        // Create new entity instance
        EntityInstance newInstance = Instantiate(data.EntitySO.EntityPrefab, EntityRoot);
        newInstance.gameObject.name = data.EntityName;

        // Attach behaviours and setup parameters
        for (int i = 0; i < data.BehaviourDatas.Count; i++)
        {
            var behaviourData = data.BehaviourDatas[i];
            var behaviourInstance = behaviourData.BehaviourSO.AddBehaviourToEntity(newInstance);
            behaviourInstance.UpdateAllParameters(behaviourData.ParamDatas);
        }

        // Register entry to the list
        EntityEntry entry = new EntityEntry() { Data = data, Instance = newInstance };
        m_EntityEntries.Add(entry);
        m_EntityEntryLookUpTable.Add(id, entry);
        newInstance.OnInstantiate(id);

        EntityIDCounter++;

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
                m_EntityEntries.RemoveAt(i);
                m_EntityEntryLookUpTable.Remove(entry.Instance.ID);
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
