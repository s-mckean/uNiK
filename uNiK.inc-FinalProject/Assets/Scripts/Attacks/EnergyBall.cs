using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Attack {

    [SerializeField] private float speed = 1.0f;

    private float radius;

	// Use this for initialization
	void Start () {
        Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, speed, speed);
        Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, speed, speed);
        radius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        StartEvent();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public override void StartEvent()
    {
        Destroy(this.gameObject, lifetime);
    }

    public override void Impact(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Stats>().ModHealth(-CalculateSplashDamage(collision));
        }
    }

    private int CalculateSplashDamage(Collider2D other)
    {
        float distanceFromCenter = Vector2.Distance(transform.position, other.bounds.ClosestPoint(transform.position));
        return (int)(baseDamage * ((radius - distanceFromCenter) / radius));
    }
}
