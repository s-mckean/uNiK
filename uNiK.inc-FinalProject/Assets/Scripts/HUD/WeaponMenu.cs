using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenu : MonoBehaviour {

    private RawImage m_Image;
    private float m_FadeTarget;
    private float m_FadeDeltaAlpha;
    private bool m_MenuOpen;
    private bool m_MenuInTransition;
    private float m_KeyDelay;
    private float m_TimeLastKeyPress;

	// Use this for initialization
	void Start () {
        m_Image = GetComponent<RawImage>();
        m_KeyDelay = 0.1f;
        m_TimeLastKeyPress = Time.time;
        m_MenuInTransition = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q) && 
            Time.time - m_TimeLastKeyPress >= m_KeyDelay
            && !m_MenuInTransition)
        {
            ToggleMenu();
            m_TimeLastKeyPress = Time.time;
        }
	}

    public void ToggleMenu()
    {
        if (m_MenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        m_Image.enabled = true;
        m_FadeTarget = 1.0f;
        m_FadeDeltaAlpha = 0.2f;
        m_MenuOpen = true;
        StartCoroutine(FadeMenu());
    }

    public void CloseMenu()
    {
        m_FadeTarget = 0.0f;
        m_FadeDeltaAlpha = -0.2f;
        m_MenuOpen = false;
        StartCoroutine(FadeMenu());
    }

    private IEnumerator FadeMenu()
    {
        m_MenuInTransition = true;

        while (true)
        {
            if (m_Image.color.a - m_FadeTarget == 0.0f)
            {
                m_MenuInTransition = false;
                CheckClosedMenu();
                yield break;
            }
            else
            {
                float alpha = m_Image.color.a + m_FadeDeltaAlpha;
                float red = m_Image.color.r;
                float green = m_Image.color.g;
                float blue = m_Image.color.b;

                m_Image.color = new Color(red, green, blue, alpha);

                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void CheckClosedMenu()
    {
        if (!m_MenuOpen)
        {
            m_Image.enabled = false;
        }
    }
}
