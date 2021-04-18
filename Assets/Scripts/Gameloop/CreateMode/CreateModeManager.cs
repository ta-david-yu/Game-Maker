using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CreateModeManager : MonoBehaviour
{
    public enum Tool
    {
        Paint = 0,
        Select,
        NumOfTools
    }

    [Header("Globla Handler")]

    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private EntityPrefabGlobalHandler m_EntityPrefabGlobalHandler;

    [Header("Data")]

    [UnityEngine.Serialization.FormerlySerializedAs("m_BehaviourSOs")]
    [SerializeField]
    private List<BehaviourTypeSO> m_BehaviourTypes;

    [Header("Reference")]

    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Transform m_EntityRoot;

    [SerializeField]
    private EntityInstance m_SelectedEntityInstance;

    [SerializeField]
    private int m_PrefabIndex = 0;

    [Header("Settings")]

    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("m_LayerMask")]
    private LayerMask m_CreateLayerMask;

    private void Awake()
    {
        m_SceneGlobalHandler.CreateNewScene();
        m_SceneGlobalHandler.EntityRoot = m_EntityRoot;
    }

    private void OnDestroy()
    {
        m_SceneGlobalHandler.EntityRoot = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_CreateLayerMask))
            {
                var entry = m_SceneGlobalHandler.CreateEntity(m_EntityPrefabGlobalHandler.EntityDatas[m_PrefabIndex]);
                entry.Instance.transform.position = hit.point;
            }
        }
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope(new GUIStyle("box"), GUILayout.Width(240)))
        {
            // Tools
            using (new GUILayout.HorizontalScope(new GUIStyle("box")))
            {
                //for (int i = 0; i < )
            }

            GUILayout.Space(24);

            // Create Button
            for (int i = 0; i < m_EntityPrefabGlobalHandler.EntityDatas.Count; i++)
            {
                var entityData = m_EntityPrefabGlobalHandler.EntityDatas[i];

                using (new GUILayout.VerticalScope())
                {
                    if (GUILayout.Button($"Choose {entityData.EntityName}"))
                    {
                        m_PrefabIndex = i;
                    }
                }
            }

            GUILayout.Space(24);

            // Entity Hierarchy
            using (new GUILayout.VerticalScope())
            {
                if (GUILayout.Button("New Scene"))
                {
                    m_SceneGlobalHandler.CreateNewScene();
                }

                for (int i = 0; i < m_SceneGlobalHandler.EntityEntries.Count; i++)
                {
                    var entry = m_SceneGlobalHandler.EntityEntries[i];

                    using (new GUILayout.HorizontalScope(new GUIStyle("box")))
                    {
                        if (GUILayout.Button("x"))
                        {
                            m_SceneGlobalHandler.DeleteEntity(entry);
                            break;
                        }

                        var originalGUIColor = GUI.color;
                        GUI.color = (entry.Instance == m_SelectedEntityInstance) ? Color.yellow : originalGUIColor;
                        if (GUILayout.Button($"{entry.Data.EntityName}", GUILayout.MaxWidth(180)))
                        {
                            m_SelectedEntityInstance = entry.Instance;
                        }
                        GUI.color = originalGUIColor;
                    }
                }
            }

            if (m_SelectedEntityInstance)
            {
                using (new GUILayout.VerticalScope(new GUIStyle("box")))
                {
                    //GUILayout.Label($"{m_SelectedEntityInstance.}")
                }
            }
        }
    }
}
