using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour runtime script
/// </summary>
public abstract class BehaviourInstanceBase : MonoBehaviour
{
    protected BehaviourSO m_BehaviourSO;
    public BehaviourSO BehaviourSO { get { return m_BehaviourSO; } }

    protected EntityInstance m_Entity;
    public EntityInstance Entity { get { return m_Entity; } }

    /// <summary>
    /// Used in create mode, called when instantiated
    /// </summary>
    /// <param name="data"></param>
    public void OnCreated(BehaviourSO data)
    {
        m_BehaviourSO = data;
    }

    /// <summary>
    /// Update all the parameters of this behaviour
    /// </summary>
    /// <param name="parameters">A list of parameter datas</param>
    public virtual void UpdateAllParameters(List<BehaviourData.BehaviourParamData> parameterDatas) { }

    /// <summary>
    /// Update a parameter
    /// </summary>
    /// <param name="parameter"></param>
    public virtual void UpdateParameter(BehaviourData.BehaviourParamData parameterData) { }

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
