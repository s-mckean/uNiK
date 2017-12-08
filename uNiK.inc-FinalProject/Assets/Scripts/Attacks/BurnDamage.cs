using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDamage : MonoBehaviour {

    [SerializeField] private int damage = 1;
    [SerializeField] private float delay = 2.0f;

    private float lastDamageTime;

    private void Start()
    {
        lastDamageTime = 0f;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Stats s = other.GetComponent<Stats>();
        Debug.Log(other);
        if (s && Time.time - lastDamageTime >= delay)
        {
            Debug.Log("burn");
            s.ModHealth(-damage);
            lastDamageTime = Time.time;
        }
    }
}
