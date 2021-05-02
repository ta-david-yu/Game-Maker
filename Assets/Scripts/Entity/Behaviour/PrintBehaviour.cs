using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBehaviour : BehaviourInstanceBase, IClickable
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

    public void OnClick()
    {
        Debug.Log($"Message: {m_Message}");
    }

    public override void OnUpdate(float timeStep)
    {
        // Do nothing
    }
}
