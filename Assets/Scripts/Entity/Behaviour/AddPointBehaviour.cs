using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointBehaviour : BehaviourInstanceBase, IClickable
{
    [SerializeField]
    private GameScoreGlobalHandler m_GameScoreGlobalHandler;

    [SerializeField]
    private BehaviourParamSO m_PointValueParameterSO;

    private int m_PointValue = 0;

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_PointValueParameterSO.GetInstanceID())
        {
            if (!int.TryParse(parameterData.Value, out m_PointValue))
            {
                m_PointValue = 0;
                Debug.LogError($"Cannot convert paramter.Value {parameterData.Value} to an integer");
            }
        }
    }

    public override void OnUpdate(float timeStep)
    {
    }

    public void OnClick()
    {
        m_GameScoreGlobalHandler.Score += m_PointValue;
    }
}
