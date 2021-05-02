using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBehaviour : BehaviourInstanceBase
{
    [SerializeField]
    private BehaviourParamSO m_ColliderTypeParameterSO;

    public Collider Collider { get; private set; } = null;

    public override void UpdateParameter(BehaviourData.BehaviourParamData parameterData)
    {
        if (parameterData.BehaviourParamSO.GetInstanceID() == m_ColliderTypeParameterSO.GetInstanceID())
        {
            if (Collider)
            {
                Destroy(Collider);
            }
            
            if (parameterData.Value == "Box")
            {
                Collider = m_Entity.gameObject.AddComponent<BoxCollider>();
            }
            else if (parameterData.Value == "Sphere")
            {
                Collider = m_Entity.gameObject.AddComponent<SphereCollider>();
            }
        }
    }

    public override void OnUpdate(float timeStep)
    {
    }

    protected override void onDetached(EntityInstance entity)
    {
        // Remove collider from the entity
        if (Collider)
        {
            Destroy(Collider);
        }
    }
}
