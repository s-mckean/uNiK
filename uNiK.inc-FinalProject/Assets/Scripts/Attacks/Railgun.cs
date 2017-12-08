using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Attack {
    private AudioSource sound;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
        StartEvent();
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity *= 1.5f;
    }

    public override void StartEvent()
    {
        sound.Play();
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
