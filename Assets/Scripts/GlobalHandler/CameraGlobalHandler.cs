using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Global Handler", menuName = "Global Handler/Camera Global Handler")]
public class CameraGlobalHandler : ScriptableObject
{
    public Camera Camera { get; private set; }

    public void RegisterCamera(Camera cam)
    {
        Camera = cam;
    }

    public void DeregisterCamera(Camera cam)
    {
        if (Camera == cam)
        {
            Camera = null;
        }
    }
}
