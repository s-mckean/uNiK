using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour {

    [SerializeField] private GameObject explosion;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(explosion, other.transform.position, Quaternion.identity);
    }
}
