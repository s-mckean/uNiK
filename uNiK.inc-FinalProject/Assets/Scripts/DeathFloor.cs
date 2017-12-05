using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathFloor : MonoBehaviour {
    public GameObject tank;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Death floor hit!");
            
            tank.GetComponent<Stats>().ModHealth(-200);
            //Destroy(this.gameObject);
            //gameObject.GetComponent<Stats>().ModHealth(-200);
        }
    }
}
