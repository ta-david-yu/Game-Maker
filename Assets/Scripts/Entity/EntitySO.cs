using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO holding entity information
/// </summary>
[CreateAssetMenu(fileName = "Entity SO", menuName = "Game Data/Entity SO")]
public class EntitySO : ScriptableObject
{
    [SerializeField]
    private string m_EntityName = "Entity";
    public string EntityName { get { return m_EntityName; } }

    [SerializeField]
    private EntityInstance m_EntityPrefab;
    public EntityInstance EntityPrefab { get { return m_EntityPrefab; } }
}
