using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour runtime script base
/// </summary>
public abstract class BehaviourBase : MonoBehaviour
{
    protected BehaviourSO m_BehaviourData;
    public BehaviourSO BehaviourData { get { return m_BehaviourData; } }

    protected EntityInstance m_Entity;
    public EntityInstance Entity { get { return m_Entity; } }

    /// <summary>
    /// Used in create mode, called when instantiated
    /// </summary>
    /// <param name="data"></param>
    public void OnCreated(BehaviourSO data)
    {
        m_BehaviourData = data;
    }

    /// <summary>
    /// Used in create mode, called when attached to an entity
    /// </summary>
    /// <param name="entity"></param>
    public void OnAttached(EntityInstance entity)
    {
        m_Entity = entity;
        onAttached(entity);
    }

    /// <summary>
    /// Used in create mode, called when detached from an entity
    /// </summary>
    /// <param name="entity"></param>
    public void OnDetached(EntityInstance entity)
    {
        onDetached(entity);
        m_Entity = null;
    }

    protected abstract void onAttached(EntityInstance entity);

    protected abstract void onDetached(EntityInstance entity);

    /// <summary>
    /// Used in play mode
    /// </summary>
    /// <param name="timeStep"></param>
    public abstract void OnUpdate(float timeStep);

    /// <summary>
    /// Used in play mode
    /// </summary>
    public abstract void OnClick();
}
