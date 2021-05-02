using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterCameraToGlobalHandler : MonoBehaviour
{
    [SerializeField]
    private CameraGlobalHandler m_GlobalHandler;

    [SerializeField]
    private Camera m_Camera;

    private void Reset()
    {
        m_Camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        m_GlobalHandler.RegisterCamera(m_Camera);
    }

    private void OnDisable()
    {
        m_GlobalHandler.DeregisterCamera(m_Camera);
    }
}
