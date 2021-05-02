using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public class PlayModeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameLoopSystemBase> m_GameLoopSystems = new List<GameLoopSystemBase>();

    public UnityEvent OnEnterPlayMode;
    public UnityEvent OnExitPlayMode;

    private bool m_IsPlaying = false;
    public bool IsPlaying { get { return m_IsPlaying; } }

    public void EnterPlayMode()
    {
        m_IsPlaying = true;

        for (int i = 0; i < m_GameLoopSystems.Count; i++)
        {
            var gameLoopSystem = m_GameLoopSystems[i];
            gameLoopSystem.EnterPlayMode();
        }

        OnEnterPlayMode.Invoke();
    }

    public void ExitPlayMode()
    {
        m_IsPlaying = false;

        for (int i = 0; i < m_GameLoopSystems.Count; i++)
        {
            var gameLoopSystem = m_GameLoopSystems[i];
            gameLoopSystem.ExitPlayMode();
        }

        OnExitPlayMode.Invoke();
    }

    public void Update()
    {
        if (IsPlaying)
        {
            float timeStep = Time.deltaTime;

            for (int i = 0; i < m_GameLoopSystems.Count; i++)
            {
                var gameLoopSystem = m_GameLoopSystems[i];
                gameLoopSystem.UpdateSystem(timeStep);
            }
        }
    }
}
