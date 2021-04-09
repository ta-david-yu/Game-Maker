using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO holding entity information
/// </summary>
[CreateAssetMenu(fileName = "Entity SO", menuName = "Game Data/Entity SO")]
public class EntitySO : ScriptableObject
{
    [UnityEngine.Serialization.FormerlySerializedAs("m_EntityName")]
    [SerializeField]
    private string m_EntityDefaultName = "Entity";
    public string EntityDefaultName { get { return m_EntityDefaultName; } }

    [SerializeField]
    private EntityInstance m_EntityPrefab;
    public EntityInstance EntityPrefab { get { return m_EntityPrefab; } }
}
