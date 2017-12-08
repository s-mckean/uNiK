using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;

    private bool paused;

	// Use this for initialization
	void Start () {
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
	}

    public void PauseGame()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        if (paused)
        {
            Time.timeScale = 0;
            TurnTimer.Instance.PauseTimer();
        }
        else
        {
            Time.timeScale = 1;
            TurnTimer.Instance.RunTimer();
        }
        TurnSystem.Instance.m_ActiveCharacter.GetComponentInChildren<GenericAim>().enabled = !paused;
        TurnSystem.Instance.m_ActiveCharacter.GetComponentInChildren<TankController>().IsActive = !paused;
    }

    public void ResumeButton()
    {
        PauseGame();
    }

    public void SwitchScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
