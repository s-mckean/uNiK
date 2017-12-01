using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerAndFuelController : MonoBehaviour {

    [SerializeField] private Text m_FuelText;
    [SerializeField] private Text m_TimerText;

    private SpriteRenderer m_SpriteRenderer;

	// Use this for initialization
	void Start () {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateTimerText());
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateTimerText());
    }

    public void SetBarColor(Color newColor)
    {
        m_SpriteRenderer.color = newColor;
    }

    private IEnumerator UpdateTimerText()
    {
        TurnTimer m_Timer = TurnTimer.Instance;

        if (m_Timer == null)
        {
            yield break;
        }

        while (true)
        {
            m_TimerText.text = "Timer: " + m_Timer.GetCurrentTime();

            if (m_Timer.FreezeTimer)
            {
                m_TimerText.text += " (frozen)";
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
