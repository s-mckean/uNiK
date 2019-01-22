using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanCamera : MonoBehaviour {

    [SerializeField] private GameObject bars;
    [SerializeField] private GameObject hpBars;

    private Animator m_Animator;
    private Camera m_Cam;

	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
        m_Cam = GetComponent<Camera>();
        StartCoroutine(BeginPanEvent());
    }

    private IEnumerator BeginPanEvent()
    {
        yield return new WaitForSeconds(1.15f);
        TurnSystem.Instance.ActivateCharacter(TurnSystem.Instance.m_ActiveCharacter, false);

        yield return new WaitForSeconds(14.0f);
        m_Cam.depth = 0;
        hpBars.SetActive(true);
        bars.SetActive(false);
        TurnTimer.Instance.RunTimer();
        TurnSystem.Instance.ActivateCharacter(TurnSystem.Instance.m_ActiveCharacter, true);
    }
}
