using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Attack {

    private void Start()
    {
        
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    }

    public override void StartEvent()
    {
        Destroy(this.gameObject, lifetime);
    }

    public override void Impact(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Stats>().ModHealth(-baseDamage);
        }
    }
}
