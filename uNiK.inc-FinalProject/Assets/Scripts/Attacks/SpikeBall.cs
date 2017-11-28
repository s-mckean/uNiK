using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

    [SerializeField] private float m_Damage;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;
    private float m_TimeSpawned;

    // Use this for initialization
    void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        m_TimeSpawned = Time.time;
    }

    private void EnableCollider() 
    {
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update () {
        m_Animator.speed = Mathf.Clamp01(m_Rigidbody2D.velocity.magnitude);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time - m_TimeSpawned > 1f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 3.0f);
        }
    }
}
