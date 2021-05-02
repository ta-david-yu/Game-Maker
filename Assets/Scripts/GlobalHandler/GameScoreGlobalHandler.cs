using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A global score handler that keeps tracks of the game score in playmode
/// TODO: should be replaced with a global variable handler in the future
/// </summary>
[CreateAssetMenu(fileName = "Game Score Global Handler", menuName = "Global Handler/Game Score Global Handler")]
public class GameScoreGlobalHandler : ScriptableObject
{
    [SerializeField]
    [Tooltip("Initial score in playmode")]
    private int m_InitialScore = 0;

    [System.NonSerialized]
    private int m_Score = 0;
    public int Score
    {
        get { return m_Score; }
        set
        {
            var prev = m_Score;
            if (prev != value)
            {
                m_Score = value;
                OnScoreChanged.Invoke(prev, value);
            }
        }
    }

    public UnityEvent<int, int> OnScoreChanged;

    public void InitScore()
    {
        Score = m_InitialScore;
    }
}
