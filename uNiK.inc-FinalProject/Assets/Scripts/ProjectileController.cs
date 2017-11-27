using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public GameObject hitbox;       // Moved from ProjectileImpact to here
    private bool rotateRound;

	// Use this for initialization
	void Start () {
        rotateRound = true;
	}
	
	// Update is called once per frame
	void Update () {
        //Vector2 veloc = GetComponent<Rigidbody2D>().velocity;
        //if (rotateRound)
        //    transform.rotation = Quaternion.LookRotation(veloc, Vector2.up);
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Impact();
        Destroy(this.gameObject);
    }

    private void Impact()
    {
        Instantiate(hitbox, transform.position, Quaternion.identity);
    }
}
