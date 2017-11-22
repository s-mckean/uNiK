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

    public void SetBarColor(Color newColor)
    {
        m_SpriteRenderer.color = newColor;
    }
}
