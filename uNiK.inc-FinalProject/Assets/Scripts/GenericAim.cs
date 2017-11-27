using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAim : MonoBehaviour {

    [SerializeField] private Transform m_Crosshair;
    [SerializeField] private Transform firingPosition;
    [SerializeField] private GameObject cam;
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private float minPower = 10f;
    [SerializeField] private float maxPower = 50f;
    [SerializeField] private float chargeTime = 0.75f;

    private string fireButton = "Fire1";
    private float chargeSpeed;
    private float currentPower;

    // Use this for initialization
    void Start () {
        chargeSpeed = (maxPower - minPower) / chargeTime;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        AdjustCrosshair();
        if (Input.GetButtonDown(fireButton)) //checks if we have pressed the firing button or not (first time firing)
        {
            currentPower = minPower;
        }
        else if (Input.GetButtonUp(fireButton)) //checks to see if the button is released, but we have not fired yet
        {
            Fire();
        }
        else if (Input.GetButton(fireButton)) //checks if the firing button is held down, but we haven't fired yet
        {
            currentPower += chargeSpeed * Time.deltaTime;
            Mathf.Clamp(currentPower, minPower, maxPower);
        }
    }

    private void AdjustCrosshair()
    {
        Vector3 mousePos = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = Vector2.MoveTowards(gameObject.GetComponent<Transform>().position,
            mousePos, 2f);
        m_Crosshair.position = newPos;
    }

    private void Fire()
    {
        Vector2 direction = m_Crosshair.position - transform.position;
        Rigidbody2D formProjectile = Instantiate(projectile, firingPosition.position, firingPosition.rotation) as Rigidbody2D;
        formProjectile.velocity = currentPower * (direction / direction.magnitude);
    }
}
