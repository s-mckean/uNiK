using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unikincTanks;

public class GameCharacter : MonoBehaviour {

    [SerializeField] private GameObject[] m_ComponentObjects;
    [SerializeField] private Stats m_Stats;

    private string m_PlayerName;
    private Teams m_PlayerTeam;


    private void Start()
    {
        m_PlayerName = "Player";
    }

    public void ActivateCharacter(bool activate)
    {
        foreach (GameObject obj in m_ComponentObjects)
        {
            obj.SetActive(activate);
        }
    }
    
    public string PlayerName
    {
        get
        {
            return this.m_PlayerName;
        }
        set
        {
            this.m_PlayerName = value;
        }
    }

    public Stats GetStats()
    {
        return this.m_Stats;
    }

    public Teams PlayerTeam
    {
        get
        {
            return this.m_PlayerTeam;
        }
        set
        {
            m_PlayerTeam = value;
        }
    }
}
