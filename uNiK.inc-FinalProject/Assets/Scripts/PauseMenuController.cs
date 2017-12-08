using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject bars;

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
            TurnTimer.Instance.StartTimer();
        }
        TurnSystem.Instance.ActivateCharacter(TurnSystem.Instance.m_ActiveCharacter, !paused);
    }

    public void ResumeButton()
    {
        PauseGame();
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
