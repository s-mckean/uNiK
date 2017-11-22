using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAim : MonoBehaviour {

    [SerializeField] private Transform m_Crosshair;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        AdjustCrosshair();
    }

    private void AdjustCrosshair()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = Vector2.MoveTowards(gameObject.GetComponent<Transform>().position,
            mousePos, 2f);
        m_Crosshair.position = newPos;
    }
}
