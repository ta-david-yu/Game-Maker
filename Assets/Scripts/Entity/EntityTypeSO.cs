using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO holding entity information
/// </summary>
[CreateAssetMenu(fileName = "EntityType SO", menuName = "Game Data/EntityType SO")]
public class EntityTypeSO : ScriptableObject
{
    [UnityEngine.Serialization.FormerlySerializedAs("m_EntityName")]
    [SerializeField]
    private string m_EntityDefaultName = "Entity";
    public string EntityDefaultName { get { return m_EntityDefaultName; } }

    [SerializeField]
    private EntityInstance m_EntityPrefab;
    public EntityInstance EntityPrefab { get { return m_EntityPrefab; } }
}
