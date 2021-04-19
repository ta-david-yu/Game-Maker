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

    [Header("Settings")]

    [SerializeField]
    private float m_SceneHierarchyWindowHeight = 350;

    [SerializeField]
    private float m_InspectorFieldNameWidth = 80;

    [Header("Runtime")]

    [SerializeField]
    private EntityInstance m_SelectedEntityInstance;

    private Vector2 m_SceneHierarchyScrollPos;

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

            // Entity Inspector
            if (m_SelectedEntityInstance)
            {
                using (new GUILayout.VerticalScope(new GUIStyle("box")))
                {
                    var entry = m_SceneGlobalHandler.GetEntityEntry(m_SelectedEntityInstance.ID);

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("ID", GUILayout.Width(m_InspectorFieldNameWidth));
                        GUILayout.Label(entry.Instance.ID.ToString());
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Name", GUILayout.Width(m_InspectorFieldNameWidth));
                        entry.Data.EntityName = GUILayout.TextField(entry.Data.EntityName);
                    }

                    for (int i = 0; i < entry.Data.BehaviourDatas.Count; i++)
                    {
                        using (new GUILayout.VerticalScope(new GUIStyle("box")))
                        {
                            var behaviourData = entry.Data.BehaviourDatas[i];
                            var behaviourInstance = entry.Instance.Behaviours[i];
                            GUILayout.Label(behaviourData.BehaviourSO.BehaviourName);

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

                                        // Change parameter value, update instance's parameter
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
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
