using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_SelectionCursorObjRoot;

    [SerializeField]
    private MeshRenderer m_SelectionCursorRenderer;

    public void HandleOnSelectedEntityChanged(EntityInstance entityInstance)
    {
        if (entityInstance)
        {
            m_SelectionCursorObjRoot.transform.position = entityInstance.transform.position;
            m_SelectionCursorRenderer.enabled = true;
        }
        else
        {
            m_SelectionCursorRenderer.enabled = false;
        }
    }
}
