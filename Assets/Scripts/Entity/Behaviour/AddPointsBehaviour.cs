using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointsBehaviour : BehaviourInstanceBase, IClickable
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
                // TODO: Error, type parsing should be done in the parameter interface
            }
        }
    }

    public override void OnUpdate(float timeStep)
    {
    }

    public void OnClick()
    {
        m_GameScoreGlobalHandler.Score += m_PointValue;
        m_Entity.gameObject.SetActive(false);
    }
}
