using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    private Rigidbody2D[] itemsList;

    // Use this for initialization
    void Start () {
        itemsList = Resources.LoadAll<Rigidbody2D>("Items");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void spawnItem()
    {
        
    }
}
