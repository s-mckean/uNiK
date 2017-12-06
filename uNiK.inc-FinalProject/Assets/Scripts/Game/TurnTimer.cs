using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(TurnSystem))]

public class TurnTimer : MonoBehaviour {
    
    [SerializeField] private int m_TurnTimerDuration = 20;
    [SerializeField] private bool m_FreezeTimer = false;

    private int m_CurrentTimerTime;
    private bool m_Paused;

    public static TurnTimer Instance;

    private void Start()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    public IEnumerator StartTimer()
    {
        ResetTimer();
        
        while (true)
        {
            if (!m_FreezeTimer && !m_Paused)
            {
                m_CurrentTimerTime--;
            }

            if (CheckZero())
            {
                ResetTimer();
                TurnSystem.Instance.Event_TimeRanOut();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private bool CheckZero()
    {
        if (m_CurrentTimerTime <= 0)
        {
            m_CurrentTimerTime = 0;
            return true;
        }

        return false;
    }

    public void ResetTimer()
    {
        m_CurrentTimerTime = m_TurnTimerDuration;
    }

    public void PauseTimer()
    {
        m_Paused = true;
    }

    public void RunTimer()
    {
        m_Paused = false;
    }

    public int GetCurrentTime()
    {
        return this.m_CurrentTimerTime;
    }

    public int TimerDuration
    {
        get
        {
            return this.m_TurnTimerDuration;
        }
        set
        {
            this.m_TurnTimerDuration = value;
        }
    }

    public bool FreezeTimer
    {
        get
        {
            return this.m_FreezeTimer;
        }
        set
        {
            m_FreezeTimer = value;
        }
    }
}
