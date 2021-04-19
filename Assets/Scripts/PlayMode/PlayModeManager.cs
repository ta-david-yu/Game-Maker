using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeManager : MonoBehaviour
{
    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private BehaviourCollectionSO m_BehaviourCollection;

    [Header("Reference")]

    [SerializeField]
    private Camera m_Camera;

    [Header("Settings")]

    [SerializeField]
    private LayerMask m_ClickLayerMask;

    private List<BehaviourTypeSO> m_RuntimeBehaviourTypes;

    private bool m_IsPlaying = false;
    public bool IsPlaying { get { return m_IsPlaying; } }

    public void EnterPlayMode()
    {
        m_IsPlaying = true;
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
        m_IsPlaying = false;
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

            // update behaviours
            for (int i = 0; i < m_RuntimeBehaviourTypes.Count; i++)
            {
                var type = m_RuntimeBehaviourTypes[i];
                type.UpdateBehaviours(timeStep);
            }

            // click event
            if (Input.GetMouseButtonDown(0))
            {
                var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_ClickLayerMask))
                {
                    EntityInstance entity;
                    if (hit.collider.TryGetComponent(out entity))
                    {
                        for (int i = 0; i < entity.Behaviours.Count; i++)
                        {
                            var behaviour = entity.Behaviours[i];
                            behaviour.OnClick();
                        }
                    }
                }
            }
        }
    }
}
