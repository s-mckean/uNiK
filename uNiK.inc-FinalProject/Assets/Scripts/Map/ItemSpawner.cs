using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    private Rigidbody2D[] itemsList;
    private int ctr = 0;

    // Use this for initialization
    void Start () {
        itemsList = Resources.LoadAll<Rigidbody2D>("Items");
    }

    private void Update()
    {
        if (ctr < 5) SpawnItem(); ctr++; 
        float tempX = transform.position.x;
        tempX += 10;
        transform.position = new Vector2(tempX, transform.position.y);
    }

    public void SpawnItem()
    {
        int itemPos = Random.Range(0, itemsList.Length);
        Instantiate(itemsList[itemPos], transform.position, Quaternion.identity);
    }
}
