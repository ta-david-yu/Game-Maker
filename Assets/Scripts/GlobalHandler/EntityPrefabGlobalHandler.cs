using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Keep tracks of all the saved prefab assets. A prefab asset is saved as an EntityData
/// </summary>
[CreateAssetMenu(fileName = "EntityPrefab Global Handler", menuName = "Global Handler/EntityPrefab Global Handler")]
public class EntityPrefabGlobalHandler : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("m_DefaultEntityPrefabs")]
    private List<EntityData> m_DefaultEntityDatas;

    [System.NonSerialized]
    private List<EntityData> m_EntityDatas = new List<EntityData>();
    public ReadOnlyCollection<EntityData> EntityDatas { get { return m_EntityDatas.AsReadOnly(); } }

    public void OnAfterDeserialize()
    {
        m_EntityDatas = new List<EntityData>();
        m_EntityDatas.AddRange(m_DefaultEntityDatas);
    }

    public void OnBeforeSerialize()
    {
    }

    public void SavePrefab(EntityData data)
    {
        m_EntityDatas.Add(data);
    }
}
