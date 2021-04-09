using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CreateModeManager : MonoBehaviour
{
    public enum Tool
    {
        Paint,
        Select
    }

    [Header("Globla Handler")]

    [SerializeField]
    private EntityGlobalHandler m_EntityGlobalHandler;

    [Header("Data")]

    [SerializeField]
    private List<EntitySO> m_EntitySOs;

    [SerializeField]
    private List<BehaviourSO> m_BehaviourSOs;

    [Header("Reference")]

    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Tool m_Tool = Tool.Paint;

    [SerializeField]
    private EntityInstance m_SelectedEntityInstance;

    private List<EntityInstance> m_EntityInstances = new List<EntityInstance>();
    public ReadOnlyCollection<EntityInstance> EntityInstances { get { return m_EntityInstances.AsReadOnly(); } }

    private void Awake()
    {
        m_EntityGlobalHandler.EntityEntries = new List<EntityGlobalHandler.EntityEntry>();
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        m_EntityGlobalHandler.EntityEntries = new List<EntityGlobalHandler.EntityEntry>();
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope(new GUIStyle("box"), GUILayout.Width(240)))
        {
            // Create Button
            for (int i = 0; i < m_EntitySOs.Count; i++)
            {
                var entitySO = m_EntitySOs[i];

                using (new GUILayout.VerticalScope())
                {
                    if (GUILayout.Button($"Create {entitySO.EntityName}"))
                    {
                        var entry = m_EntityGlobalHandler.CreateEmptyEntity(entitySO);

                    }
                }
            }

            // Entity Hierarchy
            using (new GUILayout.VerticalScope())
            {
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
                        if (GUILayout.Button($"{entry.Data.EntitySO.EntityName}", GUILayout.MaxWidth(180)))
                        {
                            m_SelectedEntityInstance = entry.Instance;
                        }
                        GUI.color = originalGUIColor;
                    }
                }
            }
        }
    }
}
