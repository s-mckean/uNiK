using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerAndFuelController : MonoBehaviour {

    [SerializeField] private Text m_FuelText;
    [SerializeField] private Text m_TimerText;

    private SpriteRenderer m_SpriteRenderer;

    private IEnumerator Start()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(UpdateTimerText());
        StartCoroutine(UpdateFuelText());
    }

    private void OnDisable()
    {
        //StopCoroutine(UpdateTimerText());
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
            if (transform.parent.transform.parent.gameObject.GetComponent<TankController>().IsActive)
            {
                m_TimerText.text = "Timer: " + m_Timer.GetCurrentTime();

                if (m_Timer.FreezeTimer)
                {
                    m_TimerText.text += " (frozen)";
                }
            }
            else
            {
                m_TimerText.text = "Timer: ";
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator UpdateFuelText()
    {
        TankController controller = transform.parent.transform.parent.gameObject.GetComponent<TankController>();
        while (true)
        {
            if (controller.IsActive)
            {
                m_FuelText.text = "Fuel: " + (int)controller.GetCurrentFuel();

                if (controller.UnlimitedFuel)
                {
                    m_FuelText.text += " (Unlimited)";
                }
            }
            else
            {
                m_FuelText.text = "Fuel: ";
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
