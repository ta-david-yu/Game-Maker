using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(BehaviourTypeSO))]
public class BehaviourSOEditor : Editor
{
    private BehaviourTypeSO m_BehaviourSO;
    private SerializedProperty m_Parameters;

    private string m_NewParamName = "New Param";

    protected void OnEnable()
    {
        m_BehaviourSO = target as BehaviourTypeSO;
        m_Parameters = serializedObject.FindProperty(nameof(m_Parameters));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        var areaStyle = new GUIStyle(EditorStyles.helpBox);

        using (new GUILayout.HorizontalScope())
        {
            m_NewParamName = EditorGUILayout.TextField(m_NewParamName);

            string parentFolderName = "";
            try
            {
                parentFolderName = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(m_BehaviourSO))}";
            }
            catch
            {
                return;
            }

            string subFolderName = $"{m_BehaviourSO.name} Params";

            string paramFolderPath =
                parentFolderName + Path.DirectorySeparatorChar +
                subFolderName;

            string paramSOName = $"{m_BehaviourSO.name} Param - {m_NewParamName}.asset";

            string assetFullPath = paramFolderPath + Path.DirectorySeparatorChar + paramSOName;

            using (new EditorGUI.DisabledGroupScope(File.Exists(assetFullPath)))
                if (GUILayout.Button("Add new param"))
                {
                    if (!AssetDatabase.IsValidFolder(paramFolderPath))
                    {
                        AssetDatabase.CreateFolder(parentFolderName, subFolderName);
                    }

                    BehaviourParamSO asset = ScriptableObject.CreateInstance<BehaviourParamSO>();
                    asset.Name = m_NewParamName;

                    AssetDatabase.CreateAsset(asset, assetFullPath);
                    AssetDatabase.SaveAssets();

                    serializedObject.Update();
                    m_Parameters.InsertArrayElementAtIndex(m_Parameters.arraySize);
                    m_Parameters.GetArrayElementAtIndex(m_Parameters.arraySize - 1).objectReferenceValue = asset;
                    serializedObject.ApplyModifiedProperties();

                    EditorUtility.SetDirty(m_BehaviourSO);
                }
        }

        for (int i = 0; i < m_BehaviourSO.Parameters.Count; i++)
        {
            var param = m_BehaviourSO.Parameters[i];

            using (new GUILayout.VerticalScope(areaStyle))
            {
                if (param)
                {
                    var boolName = $"Show {param.name} in {m_BehaviourSO.name}";
                    bool isExpanded = EditorPrefs.GetBool(boolName, false);
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(15);
                        string toggleLabel = param.name;
                        bool newIsExpanded = EditorGUILayout.Foldout(isExpanded, toggleLabel, true);

                        if (newIsExpanded != isExpanded)
                        {
                            EditorPrefs.SetBool(boolName, newIsExpanded);
                            isExpanded = newIsExpanded;
                        }

                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("delete"))
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(param));

                            serializedObject.Update();
                            m_Parameters.GetArrayElementAtIndex(i).objectReferenceValue = null;
                            m_Parameters.DeleteArrayElementAtIndex(i);
                            serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }

                    if (isExpanded)
                    {
                        using (new EditorGUI.IndentLevelScope(1))
                        {
                            var editor = Editor.CreateEditor(param);
                            editor.DrawDefaultInspector();
                        }
                    }
                }
                else
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        EditorGUILayout.HelpBox("Missing reference", MessageType.Error);

                        if (GUILayout.Button("delete"))
                        {
                            serializedObject.Update();
                            m_Parameters.GetArrayElementAtIndex(i).objectReferenceValue = null;
                            m_Parameters.DeleteArrayElementAtIndex(i);
                            serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }
                }
            }
        }
    }
}
