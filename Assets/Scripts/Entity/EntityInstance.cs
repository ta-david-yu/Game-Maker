using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Entity runtime script
/// </summary>
public class EntityInstance : MonoBehaviour
{
    [NaughtyAttributes.ShowNonSerializedField]
    private List<BehaviourBase> m_Behaviours = new List<BehaviourBase>();

    public void AttachBehaviour(BehaviourBase behaviour)
    {
        m_Behaviours.Add(behaviour);
    }

    public void DetachBehaviour(BehaviourBase behaviour)
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
