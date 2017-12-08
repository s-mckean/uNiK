using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarsManager : MonoBehaviour
{

    private Healthbar[] m_Healthbars;

    // Use this for initialization
    void Start()
    {
        m_Healthbars = GetComponentsInChildren<Healthbar>();
        Setup();
    }

    private void Setup()
    {
        List<TeamHandler> m_Teams = TurnSystem.Instance.GetTeams();

        int healthbarIndex = 0;
        for (int teamNum = 0; teamNum < m_Teams.Count; teamNum++)
        {
            GameCharacter[] m_TeamCharacters = m_Teams[teamNum].GetGameCharacters();
            for (int charNum = 0; charNum < m_TeamCharacters.Length; charNum++)
            {
                if (m_TeamCharacters[charNum].isActiveAndEnabled)
                {
                    m_Healthbars[healthbarIndex].Character = m_TeamCharacters[charNum];
                    healthbarIndex++;
                }
                
            }
        }

        for (int i = healthbarIndex; i < m_Healthbars.Length; i++)
        {
            m_Healthbars[i].gameObject.SetActive(false);
        }
    }
}
