using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreateModeToolBase : MonoBehaviour
{
    public abstract void OnClick(Ray ray);

    public abstract void DrawGUI();
}
