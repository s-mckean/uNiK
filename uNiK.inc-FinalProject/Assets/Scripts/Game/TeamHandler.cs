using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unikincTanks;

public class TeamHandler : MonoBehaviour {

    [SerializeField] private Teams m_Team;

    private SpriteRenderer[] m_SpriteRenderers;
    private TankController[] m_TankControllers;

    // Use this for initialization
    void Start()
    {
        m_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        m_TankControllers = GetComponentsInChildren<TankController>();

        switch (m_Team)
        {
            case Teams.BLUE: SetColorOfEachTeamMember(Color.blue); break;
            case Teams.GREEN: SetColorOfEachTeamMember(Color.green); break;
            case Teams.RED: SetColorOfEachTeamMember(Color.red); break;
            case Teams.YELLOW: SetColorOfEachTeamMember(Color.yellow); break;
        }
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

    public TankController[] GetTeamMembers()
    {
        return m_TankControllers;
    }
}
