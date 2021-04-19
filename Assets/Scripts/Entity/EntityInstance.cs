using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Entity runtime script
/// </summary>
public class EntityInstance : MonoBehaviour
{
    public int ID { get; private set; }

    [SerializeField]
    private List<BehaviourInstanceBase> m_Behaviours = new List<BehaviourInstanceBase>();
    public ReadOnlyCollection<BehaviourInstanceBase> Behaviours { get { return m_Behaviours.AsReadOnly(); } }

    public bool HasBehaviour(BehaviourTypeSO behaviourType)
    {
        for (int i = 0; i < Behaviours.Count; i++)
        {
            var behaviourInstance = Behaviours[i];
            if (behaviourInstance.BehaviourSO.GetInstanceID() == behaviourType.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get behaviour instance of the given type
    /// </summary>
    /// <param name="behaviourType"></param>
    /// <returns>the instance, return null if the instance with the given type is not found</returns>
    public BehaviourInstanceBase GetBehaviour(BehaviourTypeSO behaviourType)
    {
        for (int i = 0; i < Behaviours.Count; i++)
        {
            var behaviourInstance = Behaviours[i];
            if (behaviourInstance.BehaviourSO.GetInstanceID() == behaviourType.GetInstanceID())
            {
                return behaviourInstance;
            }
        }
        return null;
    }

    public void OnInstantiate(int id)
    {
        ID = id;
    }

    public void AttachBehaviour(BehaviourInstanceBase behaviour)
    {
        m_Behaviours.Add(behaviour);
    }

    public void DetachBehaviour(BehaviourInstanceBase behaviour)
    {
        m_Behaviours.Remove(behaviour);
    }

    /// <summary>
    /// On Click in play mode
    /// </summary>
    /// <param name="eventData"></param>
    public void OnClick(PointerEventData eventData)
    {
        for (int i = 0; i < m_Behaviours.Count; i++)
        {
            m_Behaviours[i].OnClick();
        }
    }
}
