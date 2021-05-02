using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for the game loop system in playmode
/// </summary>
public abstract class GameLoopSystemBase : ScriptableObject
{
    public abstract void EnterPlayMode();

    public abstract void UpdateSystem(float timeStep);

    public abstract void ExitPlayMode();
}
