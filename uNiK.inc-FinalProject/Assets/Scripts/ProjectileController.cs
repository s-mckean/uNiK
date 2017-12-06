using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public GameObject hitbox;       // Moved from ProjectileImpact to here
    private bool rotateRound;
    private bool active;

    private float timeSpawned;

	// Use this for initialization
	void Start () {
        rotateRound = true;
        active = false;
	}

    private void Awake()
    {
        timeSpawned = Time.time;
    }

    // Update is called once per frame
    void Update () {

        if (Time.time - timeSpawned > 15.0f)
        {
            Impact();
            Destroy(this.gameObject);
        }
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Impact();
        Destroy(this.gameObject);
    }

    private void Impact()
    {
        Instantiate(hitbox, transform.position, Quaternion.identity);
    }
}
