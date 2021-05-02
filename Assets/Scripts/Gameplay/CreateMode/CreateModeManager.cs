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
    private CameraGlobalHandler m_CameraHandler;

    [SerializeField]
    private GameScoreGlobalHandler m_GameScoreGlobalHandler;

    [Header("Reference")]

    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("m_PlayModeManager")]
    private PlayModeUpdater m_PlayModeUpdater;

    [SerializeField]
    private List<CreateModeToolBase> m_Tools;

    [SerializeField]
    private Transform m_EntityRoot;

    [Header("Runtime Debug Fields. Do not modify them manually")]

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
        if (!m_PlayModeUpdater.IsPlaying)
            if (Input.GetMouseButtonDown(0))
            {
                var ray = m_CameraHandler.Camera.ScreenPointToRay(Input.mousePosition);
                m_CurrentTool.OnClick(ray);
            }
    }

    private void OnGUI()
    {
        using (new GUILayout.VerticalScope(new GUIStyle("box"), GUILayout.Width(240)))
        {
            // Play Mode
            if (m_PlayModeUpdater.IsPlaying)
            {
                if (GUILayout.Button("Stop"))
                {
                    m_PlayModeUpdater.ExitPlayMode();
                }

                // Game Score Info
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Score", GUILayout.Width(80));
                    GUILayout.Label(m_GameScoreGlobalHandler.Score.ToString());
                }
            }
            else
            {
                if (GUILayout.Button("Play"))
                {
                    m_PlayModeUpdater.EnterPlayMode();
                    return;
                }

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
                    
                    GUI.enabled = tool == m_CurrentTool;
                    using (new GUILayout.VerticalScope(new GUIStyle("box")))
                    {
                        tool.DrawGUI();
                    }

                    GUI.enabled = true;
                }
            }
        }
    }
}
