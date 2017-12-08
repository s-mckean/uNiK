using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    private Rigidbody2D[] itemsList;
    private int ctr = 0;

    // Use this for initialization
    void Start () {
        itemsList = Resources.LoadAll<Rigidbody2D>("Items");
        StartCoroutine(SpawnItems());
    }

    public void SpawnItem()
    {
        int itemPos = Random.Range(0, itemsList.Length);
        Instantiate(itemsList[itemPos], transform.position, Quaternion.identity);
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            Debug.Log("Item Spawned");
            if (ctr < 10) SpawnItem(); ctr++;
            float tempX = transform.position.x;
            tempX += Random.Range(-30, 30);
            transform.position = new Vector2(tempX, transform.position.y);

            yield return new WaitForSeconds(30f);
        }
    }
}
