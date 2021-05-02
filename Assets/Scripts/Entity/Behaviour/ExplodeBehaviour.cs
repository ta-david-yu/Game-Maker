using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBehaviour : BehaviourInstanceBase, IClickable
{
    [SerializeField]
    private BehaviourParamSO m_ExplosionTimeParameterSO;

    [SerializeField]
    private BehaviourParamSO m_ExplosionRadiusParameterSO;

    [SerializeField]
    private Transform m_ExplosionTransform;

    [SerializeField]
    private MeshRenderer m_MeshRenderer;

    [SerializeField]
    private AnimationCurve m_ExplosionCurve;

    [SerializeField]
    private Color m_Color = Color.red;

    private bool m_IsExploding = false;
    private float m_ExplosionTimer = 0;
    private MaterialPropertyBlock m_MaterialPropertyBlock;

    // Parameter Value Cache
    private float m_ExplosionTime = 1.0f;
    private float m_ExplosionRadius = 5.0f;

    private void Awake()
    {
        m_MaterialPropertyBlock = new MaterialPropertyBlock();
        m_MeshRenderer.GetPropertyBlock(m_MaterialPropertyBlock);
    }

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_ExplosionTimeParameterSO.GetInstanceID())
        {
            if (!float.TryParse(parameterData.Value, out m_ExplosionTime))
            {
                // TODO: Error
            }
        }
        else if (parameterData.BehaviourParamSO.GetInstanceID() == m_ExplosionRadiusParameterSO.GetInstanceID())
        {
            if (!float.TryParse(parameterData.Value, out m_ExplosionRadius))
            {
                // TODO: Error
            }
        }
    }

    public override void OnReload()
    {
        m_IsExploding = false;
        m_ExplosionTimer = 0;
        m_ExplosionTransform.localScale = Vector3.zero;

        m_MaterialPropertyBlock.SetColor("_Color", m_Color);
        m_MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }

    public void OnClick()
    {
        if (!m_IsExploding)
        {
            m_IsExploding = true;
            m_ExplosionTimer = 0;
            m_ExplosionTransform.localScale = Vector3.zero;

            m_MaterialPropertyBlock.SetColor("_Color", m_Color);
            m_MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
        }
    }

    public override void OnUpdate(float timeStep)
    {
        if (m_IsExploding)
        {
            m_ExplosionTimer += timeStep;
            float scale = m_ExplosionCurve.Evaluate(Mathf.Clamp01(m_ExplosionTimer / m_ExplosionTime));
            m_ExplosionTransform.localScale = Vector3.one * scale * m_ExplosionRadius;

            var col = m_Color;
            col.a = Mathf.Clamp01(1 - m_ExplosionTimer / m_ExplosionTime);
            m_MaterialPropertyBlock.SetColor("_Color", col);
            m_MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);

            if (m_ExplosionTimer >= m_ExplosionTime)
            {
                m_IsExploding = false;
                m_ExplosionTransform.localScale = Vector3.zero;
            }
        }
    }
}
