using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unikincTanks;

public class TeamHandler : MonoBehaviour {

    [SerializeField] private Teams m_Team;

    private SpriteRenderer[] m_SpriteRenderers;
    private TankController[] m_TankControllers;
    private Stats[] m_Stats;

    // Use this for initialization
    void Start()
    {
        GetActiveMembers();

        switch (m_Team)
        {
            case Teams.BLUE: SetColorOfEachTeamMember(Color.blue); break;
            case Teams.GREEN: SetColorOfEachTeamMember(Color.green); break;
            case Teams.RED: SetColorOfEachTeamMember(Color.red); break;
            case Teams.YELLOW: SetColorOfEachTeamMember(Color.yellow); break;
        }
    }

    private void GetActiveMembers()
    {
        m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        m_TankControllers = GetComponentsInChildren<TankController>();
        m_Stats = GetComponentsInChildren<Stats>();
    }

    private void SetColorOfEachTeamMember(Color teamColor)
    {
        foreach (SpriteRenderer spriteRenderer in m_SpriteRenderers)
        {
            spriteRenderer.color = teamColor;
        }
    }

    public Teams Team {
        get {
            return m_Team;
        }
        set {
            m_Team = value;
        }
    }

    public TankController[] GetTeamControllers()
    {
        return m_TankControllers;
    }

    public Stats[] GetTeamStats()
    {
        return m_Stats;
    }
}
