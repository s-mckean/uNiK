using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

    [SerializeField] private int m_Damage = 40;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;
    private float m_TimeSpawned;
    private bool stopped;

    // Use this for initialization
    void Start () {
        stopped = false;
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

    private void FixedUpdate()
    {
        if (m_Rigidbody2D.velocity == Vector2.zero && !stopped)
        {
            stopped = true;
            TurnSystem.Instance.Event_ForceStopProjectileCamera();
            TurnSystem.Instance.NextTurn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time - m_TimeSpawned > 1f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponentInChildren<Stats>().ModHealth(-m_Damage);
            Destroy(this.gameObject, 3.0f);
        }
    }
}
