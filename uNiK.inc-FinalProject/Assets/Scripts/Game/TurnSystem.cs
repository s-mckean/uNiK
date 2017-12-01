﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof (TurnTimer))]

public class TurnSystem : MonoBehaviour {

    [SerializeField] private List<TeamHandler> m_Teams;
    [SerializeField] private Camera m_ProjectileCamera;
    [SerializeField] private bool m_Freeplay = true;

    private TankController[] m_ActiveTeamControllers;
    private Stats[] m_ActiveTeamStats;
    private TankController m_ActiveCharacter;
    private TeamHandler m_ActiveTeam;
    private int m_ActiveTeamIndex;
    private int m_ActiveCharacterIndex;
    private Coroutine m_TimerCoroutine;

    public static TurnSystem Instance;

	// Use this for initialization
	void Start () {
        m_ActiveTeamIndex = 0;
        m_ActiveCharacterIndex = 0;

        Invoke("Setup", 1f);
	}

    private void Awake()
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

    private void Setup()
    {
        foreach (TeamHandler team in m_Teams)
        {
            foreach (TankController controller in team.GetTeamControllers())
            {
                ActivateCharacter(controller, false);
                IgnoreCollisionsWithOtherPlayers(controller.gameObject);
            }
        }

        m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
        m_ActiveTeamControllers = m_ActiveTeam.GetTeamControllers();
        m_ActiveTeamStats = m_ActiveTeam.GetTeamStats();
        m_ActiveCharacter = m_ActiveTeamControllers[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);

        m_TimerCoroutine = StartCoroutine(TurnTimer.Instance.StartTimer());
        m_ProjectileCamera.depth = -10;
    }

    private void IgnoreCollisionsWithOtherPlayers(GameObject player)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        foreach (TeamHandler team in m_Teams)
        {
            foreach (TankController controller in team.GetTeamControllers())
            {
                Physics2D.IgnoreCollision(playerCollider, controller.gameObject.GetComponent<Collider2D>());
            }
        }
    }

    public void NextTurn()
    {
        ActivateCharacter(m_ActiveCharacter, false);

        do
        {
            if (CheckAllDead())
            {
                Event_GameOver(null);
            }
            NextCharacter();

        } while (!m_ActiveTeamStats[m_ActiveCharacterIndex].IsAlive());

        m_ActiveCharacter = m_ActiveTeamControllers[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);

        TurnTimer.Instance.ResetTimer();

        if (!m_Freeplay)
        {
            TeamHandler lastTeam = CheckLastTeamAlive();
            if (lastTeam != null)
            {
                Event_GameOver(lastTeam);
            }
        }
        
        TurnTimer.Instance.RunTimer();
    }

    private void NextCharacter()
    {
        if (m_ActiveCharacterIndex + 1 < m_ActiveTeam.GetTeamControllers().Length)
        {
            m_ActiveCharacterIndex++;
        }
        else
        {
            do
            {
                m_ActiveTeamIndex = (m_ActiveTeamIndex + 1) % m_Teams.Count;
                m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
                m_ActiveTeamControllers = m_ActiveTeam.GetTeamControllers();
                m_ActiveTeamStats = m_ActiveTeam.GetTeamStats();
                m_ActiveCharacterIndex = 0;
            } while (m_ActiveTeamControllers.Length <= 0);
        }
    }

    private bool CheckAllDead()
    {
        foreach (TeamHandler team in m_Teams)
        {
            foreach (Stats stats in team.GetTeamStats())
            {
                if (stats.IsAlive())
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ActivateCharacter(TankController tankController, bool active)
    {
        if (active)
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 5;
            tankController.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        } 
        else
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 0;
            tankController.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        ActivateTankControls(tankController, active);
    }

    private void ActivateTankControls(TankController tankController, bool active)
    {
        tankController.IsActive = active;
        tankController.GetComponentInChildren<GameCharacter>().ActivatePlayerBar(active);

        // Temporary fix so you can't move other tanks by moving into them
        //tankController.gameObject.GetComponent<Rigidbody2D>().isKinematic = !active;

        var weapSys = tankController.GetComponentInChildren<WeaponSystem>();
        if (weapSys != null)
        {
            weapSys.ActivateSystem(active);
        }
    }

    private IEnumerator AdjustCamera(GameObject projectile)
    {
        if (m_ProjectileCamera != null)
        {
            m_ProjectileCamera.GetComponent<Transform>().SetPositionAndRotation(
                m_ActiveCharacter.gameObject.GetComponentInChildren<Camera>().GetComponent<Transform>().position,
                m_ActiveCharacter.gameObject.GetComponentInChildren<Camera>().GetComponent<Transform>().rotation);
        }
        else
        {
            ActivateTankControls(m_ActiveCharacter, true);
            yield break;
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        m_ProjectileCamera.depth = 10;
        Rigidbody2D camBdy = m_ProjectileCamera.GetComponent<Rigidbody2D>();
        float origOrthoSize = m_ProjectileCamera.orthographicSize;
        float maxHeight = rb.position.y;

        while (projectile != null && rb.velocity != Vector2.zero)
        {
            if ((projectile.GetComponent<SpriteRenderer>() != null &&
                projectile.GetComponent<SpriteRenderer>().enabled) ||
                (projectile.GetComponent<ParticleSystem>() != null &&
                projectile.GetComponent<ParticleSystem>().isPlaying))
            {
                camBdy.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            }
            else
            {
                camBdy.velocity = Vector2.zero;
            }

            if (rb.velocity.y > 0 && rb.position.y >= maxHeight)
            {
                float newOrthoSize = m_ProjectileCamera.orthographicSize + Mathf.Abs(rb.velocity.y) / 100f;
                m_ProjectileCamera.orthographicSize = Mathf.Clamp(newOrthoSize, origOrthoSize, 8f);
                maxHeight = rb.position.y;
            }

            yield return new WaitForFixedUpdate();
        }

        camBdy.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.0f);

        m_ProjectileCamera.depth = 0;
        NextTurn();
    }

    private TeamHandler CheckLastTeamAlive()
    {
        bool AtLeastOneTeamAlive = false;
        TeamHandler lastTeam = null;

        foreach (TeamHandler team in m_Teams)
        {
            foreach (Stats charStat in team.GetTeamStats())
            {
                if (charStat.IsAlive() && !AtLeastOneTeamAlive)
                {
                    AtLeastOneTeamAlive = true;
                    lastTeam = team;
                    break;
                }
                else if (charStat.IsAlive() && AtLeastOneTeamAlive)
                {
                    return null;
                }
            }
        }

        return lastTeam;
    }

    public List<TeamHandler> GetTeams()
    {
        return this.m_Teams;
    }

    public void Event_ShotFired(GameObject projectile)
    {
        EndTurn();
        StartCoroutine(AdjustCamera(projectile));
    }

    public void Event_TimeRanOut()
    {
        ActivateTankControls(m_ActiveCharacter, false);
        Invoke("NextTurn", 2.0f);
    }

    private void EndTurn()
    {
        ActivateTankControls(m_ActiveCharacter, false);
        TurnTimer.Instance.PauseTimer();
    }

    public void Event_GameOver(TeamHandler winningTeam)
    {
        ActivateTankControls(m_ActiveCharacter, false);
        StopCoroutine(m_TimerCoroutine);
        LargeCenterText centerText = LargeCenterText.Instance;

        if (winningTeam != null)
        {
            Enum team = winningTeam.Team;
            centerText.Text = team.ToString() + " Team Won";
            
        }
        else
        {
            centerText.Text = "Game Over";
        }

        switch (winningTeam.Team)
        {
            case unikincTanks.Teams.BLUE: centerText.TextColor = Color.blue; break;
            case unikincTanks.Teams.GREEN: centerText.TextColor = Color.green; break;
            case unikincTanks.Teams.RED: centerText.TextColor = Color.red; break;
            case unikincTanks.Teams.YELLOW: centerText.TextColor = Color.yellow; break;
            default: centerText.TextColor = Color.white; break;
        }

        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("GameUI"))
        {
            gObj.SetActive(false);
        }

        centerText.ShowWidescreenBars(true);
        centerText.ShowText(true);
    }
}
