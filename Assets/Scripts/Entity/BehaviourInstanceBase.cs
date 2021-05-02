using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour runtime script
/// </summary>
public abstract class BehaviourInstanceBase : MonoBehaviour
{
    protected BehaviourTypeSO m_BehaviourSO;
    public BehaviourTypeSO BehaviourSO { get { return m_BehaviourSO; } }

    protected EntityInstance m_Entity;
    public EntityInstance Entity { get { return m_Entity; } }

    /// <summary>
    /// Used in create mode, called when instantiated
    /// </summary>
    /// <param name="data"></param>
    public void OnCreated(BehaviourTypeSO data)
    {
        m_BehaviourSO = data;
    }

    /// <summary>
    /// Update all the parameters of this behaviour
    /// </summary>
    /// <param name="parameters">A list of parameter datas</param>
    public virtual void UpdateAllParameters(List<BehaviourData.BehaviourParamData> parameterDatas) 
    { 
        for (int i = 0; i < parameterDatas.Count; i++)
        {
            UpdateParameter(parameterDatas[i]);
        }
    }

    /// <summary>
    /// Update a parameter
    /// </summary>
    /// <param name="parameter"></param>
    public abstract void UpdateParameter(BehaviourData.BehaviourParamData parameterData);

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

    protected virtual void onAttached(EntityInstance entity) { }

    protected virtual void onDetached(EntityInstance entity) { }

    /// <summary>
    /// Called when entering/exiting playmode
    /// </summary>
    public virtual void OnReload() { }

    /// <summary>
    /// Used in play mode
    /// </summary>
    /// <param name="timeStep"></param>
    public abstract void OnUpdate(float timeStep);
}
