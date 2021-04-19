using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Select and inspect entity in the scene
/// </summary>
public class SelectionTool : CreateModeToolBase
{
    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private EntityPrefabGlobalHandler m_EntityPrefabGlobalHandler;

    [SerializeField]
    private BehaviourCollectionSO m_BehaviourCollection;

    [Header("Settings")]

    [SerializeField]
    private float m_SceneHierarchyWindowHeight = 350;

    [SerializeField]
    private float m_InspectorFieldNameWidth = 80;

    [Header("Runtime")]

    [SerializeField]
    private EntityInstance m_SelectedEntityInstance;

    private Vector2 m_SceneHierarchyScrollPos;
    private bool m_IsAddBehaviourListOn = false;

    public override void OnClick(Ray ray)
    {

    }

    public override void DrawGUI()
    {
        GUILayout.Label("Scene Hierarchy");

        using (new GUILayout.VerticalScope())
        {
            if (GUILayout.Button("New Scene"))
            {
                m_SceneGlobalHandler.CreateNewScene();
                m_SelectedEntityInstance = null;
            }

            // Entity Hierarchy
            using (var scrollView = new GUILayout.ScrollViewScope(m_SceneHierarchyScrollPos, new GUIStyle("box"), GUILayout.Height(m_SceneHierarchyWindowHeight)))
            {
                m_SceneHierarchyScrollPos = scrollView.scrollPosition;

                for (int i = 0; i < m_SceneGlobalHandler.EntityEntries.Count; i++)
                {
                    var entry = m_SceneGlobalHandler.EntityEntries[i];

                    using (new GUILayout.HorizontalScope(new GUIStyle("box")))
                    {
                        if (GUILayout.Button("x", GUILayout.Width(25)))
                        {
                            if (entry.Instance.GetInstanceID() == m_SelectedEntityInstance.GetInstanceID())
                            {
                                m_SelectedEntityInstance = null;
                            }
                            m_SceneGlobalHandler.DeleteEntityEntry(entry);
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

            // Entity Inspector
            if (m_SelectedEntityInstance)
            {
                var entry = m_SceneGlobalHandler.GetEntityEntry(m_SelectedEntityInstance.ID);

                using (new GUILayout.VerticalScope(new GUIStyle("box")))
                {
                    // Basic Entity Info
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("ID", GUILayout.Width(m_InspectorFieldNameWidth));
                        GUILayout.Label(m_SelectedEntityInstance.ID.ToString());
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Name", GUILayout.Width(m_InspectorFieldNameWidth));
                        entry.Data.EntityName = GUILayout.TextField(entry.Data.EntityName);
                    }

                    // Behaviours
                    for (int i = 0; i < entry.Data.BehaviourDatas.Count; i++)
                    {
                        using (new GUILayout.VerticalScope(new GUIStyle("box")))
                        {
                            var behaviourData = entry.Data.BehaviourDatas[i];
                            var behaviourInstance = entry.Instance.Behaviours[i];

                            using (new GUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("x", GUILayout.Width(25)))
                                {
                                    entry.RemoveBehaviourAt(i);
                                    break;
                                }
                                GUILayout.Label(behaviourData.BehaviourSO.BehaviourName);
                            }

                            for (int j = 0; j < behaviourData.BehaviourSO.Parameters.Count; j++)
                            {
                                var paramType = behaviourData.BehaviourSO.Parameters[j];
                                int dataIndex = behaviourData.ParamDatas.FindIndex(data => data.BehaviourParamSO.GetInstanceID() == paramType.GetInstanceID());
                                var paramData = behaviourData.ParamDatas[dataIndex];

                                using (new GUILayout.HorizontalScope())
                                {
                                    GUILayout.Label(paramType.Name, GUILayout.Width(m_InspectorFieldNameWidth));
                                    
                                    if (paramType.IsEnum)
                                    {
                                        int enumIndex = paramType.EnumValues.FindIndex(0, enumValue => enumValue == paramData.Value);

                                        int newEnumIndex = GUILayout.Toolbar(enumIndex, paramType.EnumValues.ToArray());

                                        // Changed parameter value, update instance's parameter
                                        if (newEnumIndex != enumIndex)
                                        {
                                            var newParamData = new BehaviourData.BehaviourParamData()
                                            {
                                                BehaviourParamSO = paramType,
                                                Value = paramType.EnumValues[newEnumIndex]
                                            };

                                            behaviourData.ParamDatas[dataIndex] = newParamData;
                                            behaviourInstance.UpdateParameter(newParamData);
                                        }
                                    }
                                    else
                                    {
                                        string newValue = paramData.Value;

                                        switch (paramType.Type)
                                        {
                                            case BehaviourParamSO.ParamType.Integer:
                                                newValue = GUILayout.TextArea(paramData.Value);
                                                break;
                                            case BehaviourParamSO.ParamType.Float:
                                                newValue = GUILayout.TextArea(paramData.Value);
                                                break;
                                            case BehaviourParamSO.ParamType.Bool:
                                                bool oldBoolValue = bool.Parse(paramData.Value); 
                                                bool newBoolValue = GUILayout.Toggle(oldBoolValue, "");
                                                newValue = newBoolValue.ToString();
                                                break;
                                            case BehaviourParamSO.ParamType.String:
                                                newValue = GUILayout.TextArea(paramData.Value);
                                                break;
                                        }

                                        // Changed parameter value, update instance's parameter
                                        if (newValue != paramData.Value)
                                        {
                                            var newParamData = new BehaviourData.BehaviourParamData()
                                            {
                                                BehaviourParamSO = paramType,
                                                Value = newValue
                                            };
                                            behaviourData.ParamDatas[dataIndex] = newParamData;
                                            behaviourInstance.UpdateParameter(newParamData);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Add Behaviours
                    m_IsAddBehaviourListOn = GUILayout.Toggle(m_IsAddBehaviourListOn, "Add Behaviour");
                    if (m_IsAddBehaviourListOn)
                    {
                        using (new GUILayout.VerticalScope(new GUIStyle("box")))
                        {
                            for (int i = 0; i < m_BehaviourCollection.BehaviourTypes.Count; i++)
                            {
                                var behaviourType = m_BehaviourCollection.BehaviourTypes[i];
                                if (!(behaviourType.IsUnique && entry.Instance.HasBehaviour(behaviourType)))
                                { 
                                    if (GUILayout.Button($"Add {behaviourType.BehaviourName} Behaviour"))
                                    {
                                        entry.AddBehaviour(behaviourType);
                                    }
                                }
                            }
                        }
                    }

                    // Save as Prefab
                    if (GUILayout.Button("Save as Prefab"))
                    {
                        m_EntityPrefabGlobalHandler.SavePrefab(entry.Data.Clone());
                    }
                }
            }
        }
    }
}
