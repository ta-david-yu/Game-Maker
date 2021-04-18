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

    [Header("Reference")]

    [SerializeField]
    private BehaviourParamSO m_MeshTypeParameterSO;

    [SerializeField]
    private MeshFilter m_MeshFilter;

    [Header("Settings")]

    [SerializeField]
    private List<MeshSet> m_MeshSets;

    public override void UpdateAllParameters(List<BehaviourData.BehaviourParamData> parameterDatas)
    {
        for (int i = 0; i < parameterDatas.Count; i++)
        {
            var parameterData = parameterDatas[i];
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
        }
    }

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
    }

    public override void OnClick()
    {
    }

    public override void OnUpdate(float timeStep)
    {
    }

    protected override void onAttached(EntityInstance entity)
    {
    }

    protected override void onDetached(EntityInstance entity)
    {
    }
}
