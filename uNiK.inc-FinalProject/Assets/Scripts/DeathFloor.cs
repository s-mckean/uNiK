﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathFloor : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Stats>().ModHealth(-200);
        }

        else if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }
}
