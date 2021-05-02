using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// The system that updates behaviours in the game loop in playmode
/// </summary>
[CreateAssetMenu(fileName = "Behaviour Update System", menuName = "Game Loop System/Behaviour Update System")]
public class BehaviourUpdateSystem : GameLoopSystemBase, ISerializationCallbackReceiver
{
    [SerializeField]
    private SceneGlobalHandler m_SceneGlobalHandler;

    [SerializeField]
    private BehaviourCollectionSO m_BehaviourCollection;

    [Header("Debug Fields. Do not modify them manually!")]

    [NonReorderable]
    [SerializeField]
    private List<BehaviourTypeSO> m_RuntimeBehaviourTypes;
    public ReadOnlyCollection<BehaviourTypeSO> RuntimeBehaviourTypes { get { return m_RuntimeBehaviourTypes.AsReadOnly(); } }

    public override void EnterPlayMode()
    {
        m_RuntimeBehaviourTypes = new List<BehaviourTypeSO>();

        for (int i = 0; i < m_BehaviourCollection.BehaviourTypes.Count; i++)
        {
            var type = m_BehaviourCollection.BehaviourTypes[i];

            // Only types with more than one behaviour instance are registered into playmode
            if (type.RuntimeBehaviours.Count > 0)
            {
                m_RuntimeBehaviourTypes.Add(type);
            }
        }
    }

    public override void UpdateSystem(float timeStep)
    {
        // update behaviours
        for (int i = 0; i < m_RuntimeBehaviourTypes.Count; i++)
        {
            var type = m_RuntimeBehaviourTypes[i];
            type.UpdateBehaviours(timeStep);
        }
    }

    public override void ExitPlayMode()
    {
        // Reset behaviours' parameters
        for (int i = 0; i < m_SceneGlobalHandler.EntityEntries.Count; i++)
        {
            var entityEntry = m_SceneGlobalHandler.EntityEntries[i];
            for (int j = 0; j < entityEntry.Data.BehaviourDatas.Count; j++)
            {
                var behaviourData = entityEntry.Data.BehaviourDatas[j];
                var behaviourInstance = entityEntry.Instance.Behaviours[j];
                behaviourInstance.UpdateAllParameters(behaviourData.ParamDatas);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        m_RuntimeBehaviourTypes = new List<BehaviourTypeSO>();
    }

    public void OnBeforeSerialize()
    {
    }
}
