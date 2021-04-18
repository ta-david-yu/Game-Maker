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
    private EntityGlobalHandler m_EntityGlobalHandler;

    [Header("Data")]

    [UnityEngine.Serialization.FormerlySerializedAs("m_BehaviourSOs")]
    [SerializeField]
    private List<BehaviourTypeSO> m_BehaviourTypes;

    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("m_EntityDatsa")]
    private List<EntityData> m_DefaultEntityDatas;

    [Header("Reference")]

    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Transform m_EntityRoot;

    [SerializeField]
    private Tool m_Tool = Tool.Paint;

    [SerializeField]
    private EntityInstance m_SelectedEntityInstance;

    [SerializeField]
    private int m_PaintIndex = 0;

    [Header("Settings")]

    [SerializeField]
    private LayerMask m_LayerMask;

    private List<EntityData> m_EntityPalette = new List<EntityData>();

    private void Awake()
    {
        m_EntityGlobalHandler.CreateNewScene();
        m_EntityGlobalHandler.EntityRoot = m_EntityRoot;

        m_EntityPalette.AddRange(m_DefaultEntityDatas);

        /*
        for (int i = 0; i < m_EntityTypes.Count; i++)
        {
            var entitySO = m_EntityTypes[i];
            m_EntityPalette.Add(
                new EntityData() { 
                    EntityName = entitySO.EntityDefaultName,
                    EntitySO = entitySO, 
                    BehaviourDatas = new List<BehaviourData>() });
        }*/
    }

    private void OnDestroy()
    {
        m_EntityGlobalHandler.EntityRoot = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask))
            {
                var entry = m_EntityGlobalHandler.CreateEntity(m_EntityPalette[m_PaintIndex]);
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
            for (int i = 0; i < m_EntityPalette.Count; i++)
            {
                var entityData = m_EntityPalette[i];

                using (new GUILayout.VerticalScope())
                {
                    if (GUILayout.Button($"Choose {entityData.EntityName}"))
                    {
                        m_PaintIndex = i;
                    }
                }
            }

            GUILayout.Space(24);

            // Entity Hierarchy
            using (new GUILayout.VerticalScope())
            {
                if (GUILayout.Button("New Scene"))
                {
                    m_EntityGlobalHandler.CreateNewScene();
                }

                for (int i = 0; i < m_EntityGlobalHandler.EntityEntries.Count; i++)
                {
                    var entry = m_EntityGlobalHandler.EntityEntries[i];

                    using (new GUILayout.HorizontalScope(new GUIStyle("box")))
                    {
                        if (GUILayout.Button("x"))
                        {
                            m_EntityGlobalHandler.DeleteEntity(entry);
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
