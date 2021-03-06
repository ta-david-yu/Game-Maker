using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// SO holding behaviour information
/// </summary>
[CreateAssetMenu(fileName = "BehaviourType SO", menuName = "Game Data/BehaviourType SO")]
public class BehaviourTypeSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private string m_BehaviourName;
    public string BehaviourName { get { return m_BehaviourName; } }

    [SerializeField]
    [Tooltip("An entity can only have one of this behaviour type instance")]
    private bool m_IsUnique = false;
    public bool IsUnique { get { return m_IsUnique; } }

    [SerializeField]
    [Tooltip("The actual prefab with runtime logic")]
    private BehaviourInstanceBase m_BehaviourPrefab;
    public BehaviourInstanceBase BehaviourPrefab { get { return m_BehaviourPrefab; } }

    [SerializeField]
    [Tooltip("A list of parameters for this behaviour")]
    private List<BehaviourParamSO> m_Parameters = new List<BehaviourParamSO>();
    public ReadOnlyCollection<BehaviourParamSO> Parameters { get { return m_Parameters.AsReadOnly(); } }

    [NonReorderable]
    private List<BehaviourInstanceBase> m_RuntimeBehaviours = new List<BehaviourInstanceBase>();
    public ReadOnlyCollection<BehaviourInstanceBase> RuntimeBehaviours { get { return m_RuntimeBehaviours.AsReadOnly(); } }

    /// <summary>
    /// Create behaviour data of this type with default values
    /// </summary>
    /// <returns></returns>
    public BehaviourData CreateDefaultBehaviourData()
    {
        var data = new BehaviourData()
        {
            BehaviourSO = this,
            ParamDatas = new List<BehaviourData.BehaviourParamData>()
        };

        for (int i = 0; i < Parameters.Count; i++)
        {
            var paramType = Parameters[i];
            data.ParamDatas.Add(new BehaviourData.BehaviourParamData()
            {
                BehaviourParamSO = paramType,
                Value = paramType.DefaultValue
            });
        }

        return data;
    }

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

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        m_RuntimeBehaviours = new List<BehaviourInstanceBase>();
    }

    #endregion
}
