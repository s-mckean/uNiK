using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    [SerializeField] private List<TeamHandler> m_Teams;

    private TankController[] m_CharactersInActiveTeam;
    private TankController m_ActiveCharacter;
    private TeamHandler m_ActiveTeam;
    private int m_ActiveTeamIndex;
    private int m_ActiveCharacterIndex;

	// Use this for initialization
	void Start () {
        m_ActiveTeamIndex = 0;
        m_ActiveCharacterIndex = 0;

        Invoke("Setup", 1f);
	}

    private void Setup()
    {
        foreach (TeamHandler team in m_Teams)
        {
            foreach (TankController controller in team.GetTeamMembers())
            {
                ActivateCharacter(controller, false);
            }
        }

        m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
        m_CharactersInActiveTeam = m_ActiveTeam.GetTeamMembers();
        m_ActiveCharacter = m_CharactersInActiveTeam[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);
    }

    public void NextTurn()
    {
        ActivateCharacter(m_ActiveCharacter, false);

        if (m_ActiveCharacterIndex + 1 < m_ActiveTeam.GetTeamMembers().Length)
        {
            m_ActiveCharacterIndex++;
        }
        else
        {
            do
            {
                m_ActiveTeamIndex = (m_ActiveTeamIndex + 1) % m_Teams.Count;
                m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
                m_CharactersInActiveTeam = m_ActiveTeam.GetTeamMembers();
                m_ActiveCharacterIndex = 0;
            }   while (m_CharactersInActiveTeam.Length <= 0);
        }

        m_ActiveCharacter = m_CharactersInActiveTeam[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);
    }

    private void ActivateCharacter(TankController tankController, bool active)
    {
        if (active)
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 5;
        } 
        else
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 0;
        }

        tankController.IsActive = active;
        tankController.GetComponentInChildren<GameCharacter>().ActivateCharacter(active);

        // Temporary fix so you can't move other tanks by moving into them
        tankController.gameObject.GetComponent<Rigidbody2D>().isKinematic = !active;

        var weapSys = tankController.GetComponentInChildren<WeaponSystem>();
        if (weapSys != null)
        {
            weapSys.ActivateSystem(active);
        }
    }
}
