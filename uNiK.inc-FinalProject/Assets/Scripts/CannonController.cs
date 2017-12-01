using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    [SerializeField] private Transform crosshairTransform;

    public SpriteRenderer cannon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        adjustCannon();
	}

    private void adjustCannon()
    {
        Vector3 direction = crosshairTransform.position - cannon.GetComponent<Transform>().position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        //Quaternion rotation = Quaternion.Euler(angle - 90f, angle + 90f, 0);
        //rotation = Mathf.Clamp(rotation, 90, -90);
        cannon.transform.rotation = rotation;
        //rotation = Mathf.Clamp(rotation, 90, -90);
    }
}
