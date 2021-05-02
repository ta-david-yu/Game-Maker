using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The system that casts rays from the camera and triggers IClickable.OnClick event on behaviours
/// </summary>
[CreateAssetMenu(fileName = "Click Raycaster System", menuName = "Game Loop System/Click Raycaster System")]
public class ClickRaycasterSystem : GameLoopSystemBase
{
    [SerializeField]
    private CameraGlobalHandler m_CameraHandler;

    [Header("Settings")]

    [SerializeField]
    private LayerMask m_ClickLayerMask;

    public override void EnterPlayMode()
    {
    }

    public override void ExitPlayMode()
    {
    }

    public override void UpdateSystem(float timeStep)
    {
        // click event
        if (Input.GetMouseButtonDown(0))
        {
            var ray = m_CameraHandler.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_ClickLayerMask))
            {
                EntityInstance entity;
                if (hit.collider.TryGetComponent(out entity))
                {
                    for (int i = 0; i < entity.Behaviours.Count; i++)
                    {
                        var behaviour = entity.Behaviours[i];
                        IClickable clickable;
                        if (behaviour.TryGetComponent(out clickable))
                            clickable.OnClick();
                    }
                }
            }
        }
    }
}
