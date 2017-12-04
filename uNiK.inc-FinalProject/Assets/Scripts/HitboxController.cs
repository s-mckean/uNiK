using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour {

    public GameObject innerHitbox;
    public GameObject particles;
    public float radius;
    public float outerDmgMod;
    public float innerHitboxRadius;
    public int damage;
    public bool explosion;
    public float lifetime;
    private float explosionDuration;

    // Use this for initialization
    void Start() {
        GetComponent<CircleCollider2D>().radius = radius;
        if (explosion)
        {
            explosionDuration = particles.GetComponent<ParticleSystem>().main.duration;
            SetupExplosion();
        }
        Destroy(this.gameObject, explosionDuration);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetupExplosion()
    {
        SetupInnerHitbox();
        GameObject explosion = Instantiate(particles, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, explosionDuration);
    }

    private void SetupInnerHitbox()
    {
        GameObject instanceInner = Instantiate(innerHitbox, transform.position, Quaternion.identity);
        instanceInner.GetComponent<CircleCollider2D>().radius = innerHitboxRadius;
        innerHitbox = instanceInner;
        Destroy(instanceInner, explosionDuration);
    }

    private int CalculateSplashDamage(Collider2D other)
    {
        float distanceFromCenter = (GetComponent<CircleCollider2D>().transform.position - other.bounds.ClosestPoint(transform.position)).magnitude;
        return (int)(damage * outerDmgMod * ((radius - distanceFromCenter) / radius));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = other.gameObject;
        Stats pScript = player.GetComponent<Stats>();
        if (pScript != null)
        {
            if (!explosion)
            {
                other.gameObject.GetComponent<Stats>().ModHealth(-damage);
            }
            else
            {
                if (other.bounds.Intersects(innerHitbox.GetComponent<CircleCollider2D>().bounds))
                {
                    other.gameObject.GetComponent<Stats>().ModHealth(-damage);
                }
                else if (other.bounds.Intersects(GetComponent<CircleCollider2D>().bounds))
                {
                    other.gameObject.GetComponent<Stats>().ModHealth(-CalculateSplashDamage(other));
                }
            }
        }
    }
}
