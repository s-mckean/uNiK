using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    private Rigidbody2D[] itemsList;

    // Use this for initialization
    void Start () {
        itemsList = Resources.LoadAll<Rigidbody2D>("Items");
    }

    public void SpawnItem()
    {
        int itemPos = Random.Range(0, itemsList.Length);
        Instantiate(itemsList[itemPos], transform.position, Quaternion.identity);
    }
}
