using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour {

    public float addedSpeed = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var stats = collision.gameObject.GetComponent<Stats>();
            stats.ModTankSpeed(stats.origTankSpeed + addedSpeed);
            Destroy(this.gameObject);
        }
    }
}
