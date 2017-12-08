using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaExplosion : MonoBehaviour {

    [SerializeField] private GameObject firePrefab;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(firePrefab, other.transform.position, Quaternion.identity);
    }

}
