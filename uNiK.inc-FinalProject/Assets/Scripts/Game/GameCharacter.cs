using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unikincTanks;

public class GameCharacter : MonoBehaviour {

    [SerializeField] private GameObject[] m_ComponentObjects;

	public void ActivateCharacter(bool activate)
    {
        foreach (GameObject obj in m_ComponentObjects)
        {
            obj.SetActive(activate);
        }
    }
    
}
