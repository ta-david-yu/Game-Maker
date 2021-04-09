using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBehaviour : BehaviourBase
{
    public override void OnClick()
    {
        Debug.Log($"Click on {Entity}");
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
