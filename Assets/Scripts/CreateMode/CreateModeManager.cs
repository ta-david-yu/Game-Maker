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

    [Header("Data")]

    [SerializeField]
    private BehaviourCollectionSO m_BehaviourCollection;

    [Header("Reference")]

    [SerializeField]
    private List<CreateModeToolBase> m_Tools;

    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Transform m_EntityRoot;

    [Header("Runtime")]

    [SerializeField]
    private CreateModeToolBase m_CurrentTool;

    private void Awake()
    {
        m_SceneGlobalHandler.CreateNewScene();
        m_SceneGlobalHandler.EntityRoot = m_EntityRoot;

        m_CurrentTool = m_Tools[0];
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
            m_CurrentTool.OnClick(ray);
        }
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope(new GUIStyle("box"), GUILayout.Width(240)))
        {
            // Tools
            using (new GUILayout.VerticalScope(new GUIStyle("box")))
            {
                GUILayout.Label("Tools");
                using (new GUILayout.HorizontalScope())
                {
                    var originalGUIColor = GUI.color;
                    for (int i = 0; i < m_Tools.Count; i++)
                    {
                        var tool = m_Tools[i];

                        GUI.color = (tool == m_CurrentTool) ? Color.yellow : originalGUIColor;
                        if (GUILayout.Button(tool.gameObject.name))
                        {
                            m_CurrentTool = tool;
                        }
                    }
                    GUI.color = originalGUIColor;
                }
            }

            // Draw tools GUI
            for (int i = 0; i < m_Tools.Count; i++)
            {
                var tool = m_Tools[i];
                using (new GUILayout.VerticalScope(new GUIStyle("box")))
                {
                    tool.DrawGUI();
                }
            }
        }
    }
}
