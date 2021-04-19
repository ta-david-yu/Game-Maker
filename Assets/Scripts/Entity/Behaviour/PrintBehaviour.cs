using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBehaviour : BehaviourInstanceBase
{
    [SerializeField]
    private BehaviourParamSO m_MessageParamSO;

    private string m_Message;

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_MessageParamSO.GetInstanceID())
        {
            m_Message = parameterData.Value;
        }
    }

    public override void OnClick()
    {
        Debug.Log($"Message: {m_Message}");
    }

    public override void OnUpdate(float timeStep)
    {
        // Do nothing
    }

    protected override void onAttached(EntityInstance entity)
    {
        // Do nothing
    }

    protected override void onDetached(EntityInstance entity)
    {
        // Do nothing
    }
}
