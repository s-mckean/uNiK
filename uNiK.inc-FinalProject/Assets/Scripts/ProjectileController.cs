﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public GameObject hitbox;       // Moved from ProjectileImpact to here
    private bool rotateRound;
    private bool active;

	// Use this for initialization
	void Start () {
        rotateRound = true;
        active = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Vector2 veloc = GetComponent<Rigidbody2D>().velocity;
        //if (rotateRound)
        //    transform.rotation = Quaternion.LookRotation(veloc, Vector2.up);
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Impact();
        Destroy(this.gameObject);
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    active = true;
    //}

    private void Impact()
    {
        Instantiate(hitbox, transform.position, Quaternion.identity);
    }
}
