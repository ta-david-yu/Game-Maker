using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EntityBaseBehaviours holds common data that every entity has
/// </summary>
public class EntityBaseBehaviour : BehaviourInstanceBase
{
    [SerializeField]
    private BehaviourParamSO m_IsActiveParameterSO;

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_IsActiveParameterSO.GetInstanceID())
        {
            bool isActive = true;
            if (!bool.TryParse(parameterData.Value, out isActive))
            {
                // TODO: Error:
            }
            m_Entity.gameObject.SetActive(isActive);
        }
    }

    public override void OnUpdate(float timeStep)
    {
        throw new System.NotImplementedException();
    }
}
