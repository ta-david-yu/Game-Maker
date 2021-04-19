using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// SO holding behaviour information
/// </summary>
[CreateAssetMenu(fileName = "BehaviourType SO", menuName = "Game Data/BehaviourType SO")]
public class BehaviourTypeSO : ScriptableObject
{
    [SerializeField]
    private string m_BehaviourName;
    public string BehaviourName { get { return m_BehaviourName; } }

    [SerializeField]
    [Tooltip("An entity can only have one of this behaviour type instance")]
    private bool m_IsUnique = true;
    public bool IsUnique { get { return m_IsUnique; } }

    [SerializeField]
    [Tooltip("The actual prefab with runtime logic")]
    private BehaviourInstanceBase m_BehaviourPrefab;
    public BehaviourInstanceBase BehaviourPrefab { get { return m_BehaviourPrefab; } }

    [SerializeField]
    [Tooltip("A list of parameters for this behaviour")]
    private List<BehaviourParamSO> m_Parameters = new List<BehaviourParamSO>();
    public ReadOnlyCollection<BehaviourParamSO> Parameters { get { return m_Parameters.AsReadOnly(); } }

    [System.NonSerialized]
    private List<BehaviourInstanceBase> m_RuntimeBehaviours = new List<BehaviourInstanceBase>();
    public ReadOnlyCollection<BehaviourInstanceBase> RuntimeBehaviours { get { return m_RuntimeBehaviours.AsReadOnly(); } }

    #region Create mode
    /// <summary>
    /// Used in create mode, add a behaviour to an entity instance
    /// </summary>
    public BehaviourInstanceBase AddBehaviourToEntity(EntityInstance entity)
    {
        // TODO: instead of creating a new behaviour instance, replaced with object pooling
        BehaviourInstanceBase behaviour = Instantiate(m_BehaviourPrefab, entity.transform);
        behaviour.transform.localPosition = Vector3.zero;
        behaviour.OnCreated(this);

        m_RuntimeBehaviours.Add(behaviour);
        entity.AttachBehaviour(behaviour);
        behaviour.OnAttached(entity);

        return behaviour;
    }

    /// <summary>
    /// Used in create mode, remove a behaviour from an entity instance
    /// </summary>
    public void RemoveBehaviourFromEntity(EntityInstance entity, BehaviourInstanceBase behaviour)
    {
        behaviour.OnDetached(entity);
        entity.DetachBehaviour(behaviour);
        m_RuntimeBehaviours.Remove(behaviour);

        // TODO: recycle to pool
        Destroy(behaviour.gameObject);
    }

    #endregion

    #region Play mode

    public void OnEnterPlayMode()
    {
        // TODO: serialization, reset state
    }

    /// <summary>
    /// Play mode update
    /// </summary>
    /// <param name="timeStep"></param>
    public void UpdateBehaviours(float timeStep)
    {
        for (int i = 0; i < RuntimeBehaviours.Count; i++)
        {
            var behaviour = RuntimeBehaviours[i];
            behaviour.OnUpdate(timeStep);
        }
    }

    public void OnExitPlayMode()
    {
        // TODO: reset state
    }

    #endregion
}
