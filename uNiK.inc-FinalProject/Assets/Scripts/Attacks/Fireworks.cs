using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class Fireworks : MonoBehaviour {

    [SerializeField] private GameObject explosion;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(explosion, other.transform.position, Quaternion.identity);
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        Camera projectileCam = GameObject.FindGameObjectsWithTag("ProjectileCamera")[0].GetComponent<Camera>();
        projectileCam.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
