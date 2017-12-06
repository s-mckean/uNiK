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
	
	// Update is called once per frame
	void Update () {

	}

    private IEnumerator BeginPanEvent()
    {
        yield return new WaitForSeconds(10.0f);
        m_Cam.depth = 0;
        hpBars.SetActive(true);
        bars.SetActive(false);
        TurnTimer.Instance.RunTimer();
    }
}
