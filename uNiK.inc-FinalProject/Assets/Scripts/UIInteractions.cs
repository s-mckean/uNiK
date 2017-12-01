using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewPlayerButton(GameObject playerPanel)
    {
        playerPanel.SetActive(true);
        GameManager.playerCount++;
    }

    public void RemoveButton(GameObject playerPanel)
    {
        playerPanel.SetActive(false);
        GameManager.playerCount--;
    }
}
