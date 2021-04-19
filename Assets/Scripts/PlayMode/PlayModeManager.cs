using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeManager : MonoBehaviour
{
    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private BehaviourCollectionSO m_BehaviourCollection;

    private List<BehaviourTypeSO> m_RuntimeBehaviourTypes;

    private bool m_IsPlaying = false;
    public bool IsPlaying { get { return m_IsPlaying; } }

    public void EnterPlayMode()
    {
        m_RuntimeBehaviourTypes = new List<BehaviourTypeSO>();

        for (int i = 0; i < m_BehaviourCollection.BehaviourTypes.Count; i++)
        {
            var type = m_BehaviourCollection.BehaviourTypes[i];
            if (type.RuntimeBehaviours.Count > 0)
            {
                m_RuntimeBehaviourTypes.Add(type);
                type.OnEnterPlayMode();
            }
        }
    }

    public void ExitPlayMode()
    {
        for (int i = 0; i < m_RuntimeBehaviourTypes.Count; i++)
        {
            var type = m_RuntimeBehaviourTypes[i];
            type.OnExitPlayMode();
        }
    }

    private void Update()
    {
        if (IsPlaying)
        {
            float timeStep = Time.deltaTime;

            for (int i = 0; i < m_RuntimeBehaviourTypes.Count; i++)
            {
                var type = m_RuntimeBehaviourTypes[i];
                type.UpdateBehaviours(timeStep);
            }
        }
    }
}
