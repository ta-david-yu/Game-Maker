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
    [SerializeField]
    private List<BehaviourInstanceBase> m_Behaviours = new List<BehaviourInstanceBase>();
    public ReadOnlyCollection<BehaviourInstanceBase> Behaviours { get { return m_Behaviours.AsReadOnly(); } }

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
