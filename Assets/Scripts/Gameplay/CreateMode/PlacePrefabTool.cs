using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Select and place entity prefab in the scene
/// </summary>
public class PlacePrefabTool : CreateModeToolBase
{
    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private EntityPrefabGlobalHandler m_EntityPrefabGlobalHandler;

    [Header("Settings")]

    [SerializeField]
    private float m_PrefabListWindowHeight = 350;

    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("m_LayerMask")]
    private LayerMask m_CreateLayerMask;

    public UnityEvent<EntityInstance> OnNewEntityInstanceCreated;

    [Header("Runtime Debug Fields. Do not modify them manually")]

    [SerializeField]
    private int m_CurrentPrefabIndex = 0;

    private Vector2 m_PrefabListScrollPos;

    public override void OnClick(Ray ray)
    {
        // place a new entity at the hit point
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_CreateLayerMask))
        {
            var entry = m_SceneGlobalHandler.CreateEntity(m_EntityPrefabGlobalHandler.EntityDatas[m_CurrentPrefabIndex]);
            entry.Instance.transform.position = hit.point;
            OnNewEntityInstanceCreated.Invoke(entry.Instance);
        }
    }

    public override void DrawGUI()
    {
        GUILayout.Label("Prefab List");

        var originalGUIColor = GUI.color;

        using (var scrollView = new GUILayout.ScrollViewScope(m_PrefabListScrollPos, GUILayout.Height(m_PrefabListWindowHeight)))
        {
            m_PrefabListScrollPos = scrollView.scrollPosition;

            for (int i = 0; i < m_EntityPrefabGlobalHandler.EntityDatas.Count; i++)
            {
                GUI.color = (i == m_CurrentPrefabIndex) ? Color.yellow : originalGUIColor;

                var entityData = m_EntityPrefabGlobalHandler.EntityDatas[i];

                using (new GUILayout.VerticalScope())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (m_EntityPrefabGlobalHandler.EntityDatas.Count > 1 && 
                            GUILayout.Button("x", GUILayout.Width(25)))
                        {
                            m_EntityPrefabGlobalHandler.RemovePrefab(i);
                            if (m_CurrentPrefabIndex == i)
                            {
                                m_CurrentPrefabIndex = 0;
                            }
                            break;
                        }

                        if (GUILayout.Button($"{entityData.EntityName}"))
                        {
                            m_CurrentPrefabIndex = i;
                        }
                    }
                }
            }
        }
        GUI.color = originalGUIColor;
    }
}
