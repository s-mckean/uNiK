using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ExplodingBouncyBall : MonoBehaviour {

    [SerializeField] private float m_ExplosionTimer;
    [SerializeField] private GameObject m_ExplosionObject;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;
    private GameObject m_SpawnedExplosionObj;

	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        Invoke("Explode", m_ExplosionTimer);
    }
	
	// Update is called once per frame
	void Update () {
        m_Animator.speed = Mathf.Clamp01(m_Rigidbody2D.velocity.magnitude);
	}

    private void Explode()
    {
        AudioSource boom = GetComponent<AudioSource>();

        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        m_SpawnedExplosionObj = GameObject.Instantiate(m_ExplosionObject, transform.position, Quaternion.identity);
        ParticleSystem explosion = m_ExplosionObject.GetComponent<ParticleSystem>();
        boom.Play();
        Destroy(m_SpawnedExplosionObj, explosion.main.duration);
        Destroy(this.gameObject, explosion.main.duration);
    }
}
