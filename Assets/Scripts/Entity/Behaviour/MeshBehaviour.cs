using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBehaviour : BehaviourInstanceBase
{
    [System.Serializable]
    private struct MeshSet
    {
        public string MeshName;
        public Mesh MeshAsset;
    }

    [System.Serializable]
    private struct MaterialSet
    {
        public string MaterialName;
        public Material MaterialAsset;
    }

    [Header("Reference")]

    [SerializeField]
    private BehaviourParamSO m_MeshTypeParameterSO;

    [SerializeField]
    private BehaviourParamSO m_MaterialTypeParameterSO;

    [Space]

    [SerializeField]
    private MeshFilter m_MeshFilter;

    [SerializeField]
    private MeshRenderer m_MeshRenderer;

    [Header("Settings")]

    [SerializeField]
    private List<MeshSet> m_MeshSets;

    [SerializeField]
    private List<MaterialSet> m_MaterialSets;

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_MeshTypeParameterSO.GetInstanceID())
        {
            for (int j = 0; j < m_MeshSets.Count; j++)
            {
                var set = m_MeshSets[j];
                if (set.MeshName == parameterData.Value)
                {
                    m_MeshFilter.sharedMesh = set.MeshAsset;
                    break;
                }
            }
        }
        else if (parameterData.BehaviourParamSO.GetInstanceID() == m_MaterialTypeParameterSO.GetInstanceID())
        {
            for (int j = 0; j < m_MeshSets.Count; j++)
            {
                var set = m_MaterialSets[j];
                if (set.MaterialName == parameterData.Value)
                {
                    m_MeshRenderer.sharedMaterial = set.MaterialAsset;
                    break;
                }
            }
        }
    }

    public override void OnUpdate(float timeStep)
    {
    }
}
