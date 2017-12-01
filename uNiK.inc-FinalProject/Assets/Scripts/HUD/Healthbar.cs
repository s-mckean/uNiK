using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    [SerializeField] private Image m_FillImage;
    [SerializeField] private float m_MaxValue = 0.85f;
    [SerializeField] private Text m_NameText;
    private GameCharacter m_Character;

    private Slider m_Slider;

	// Use this for initialization
	void Start () {
        m_Slider = GetComponent<Slider>();
        m_Slider.value = m_MaxValue;
        m_Slider.interactable = false;
	}

    private void Awake()
    {
        StartCoroutine(UpdateHealthbar());
    }

    private void Update()
    {
        //if (m_Character != null)
        //{
        //    Stats m_Stats = m_Character.GetStats();
        //    float currHealth = (float)m_Stats.GetHealth();
        //    float maxHealth = (float)m_Stats.GetMaxHealth();
        //    float newValue = (currHealth / maxHealth) * m_MaxValue;

        //    m_Slider.value = newValue;
        //    CheckZero();
        //}
    }

    private IEnumerator UpdateHealthbar()
    {
        while (true)
        {
            if (m_Character != null)
            {
                Stats m_Stats = m_Character.GetStats();
                float currHealth = (float)m_Stats.GetHealth();
                float maxHealth = (float)m_Stats.GetMaxHealth();
                float newValue = (currHealth / maxHealth) * m_MaxValue;

                m_Slider.value = newValue;
                CheckZero();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckZero()
    {
        if (m_Slider.value == 0)
        {
            m_FillImage.enabled = false;
            m_NameText.color = Color.grey;
        }
        else
        {
            m_FillImage.enabled = true;
            m_NameText.color = Color.white;
        }
    }

    private void SetName()
    {
        Enum teamName = m_Character.PlayerTeam;
        m_NameText.text = "[" + teamName.ToString() + "]" + m_Character.PlayerName;
    }

    public GameCharacter Character
    {
        get
        {
            return this.m_Character;
        }
        set
        {
            this.m_Character = value;
            SetName();
        }
    }
}
