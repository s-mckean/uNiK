using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LargeCenterText : MonoBehaviour {

    [SerializeField] private GameObject m_WidescreenBars;

    private Text m_Text;
    private float m_FadeTarget;
    private float m_FadeDeltaAlpha;

    public static LargeCenterText Instance;

	// Use this for initialization
	void Start () {
        m_Text = GetComponent<Text>();
	}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public string Text
    {
        get
        {
            return this.m_Text.text;
        }
        set
        {
            this.m_Text.text = value;
        }
    }

    public Color TextColor
    {
        get
        {
            return this.m_Text.color;
        }
        set
        {
            this.m_Text.color = value;
        }
    }

    public void FadeOut()
    {
        m_FadeTarget = 0f;
        m_FadeDeltaAlpha = -0.01f;
        StartCoroutine(Fade());
    }

    public void FadeIn()
    {
        m_FadeTarget = 1f;
        m_FadeDeltaAlpha = 0.01f;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            if (m_Text.color.a - m_FadeTarget == 0.0f)
            {
                yield break;
            }
            else
            {
                float alpha = m_Text.color.a + m_FadeDeltaAlpha;
                float red = m_Text.color.r;
                float green = m_Text.color.g;
                float blue = m_Text.color.b;

                m_Text.color = new Color(red, green, blue, alpha);

                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void ShowWidescreenBars(bool show)
    {
        m_WidescreenBars.SetActive(show);
    }

    public void ShowText(bool show)
    {
        m_Text.enabled = show;
    }
}
