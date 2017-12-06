using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour {

    [SerializeField] private float m_BaseDamage = 70f;

    private ParticleSystem m_Particle;
    private List<GameObject> toIgnore;

    private void Start()
    {
        m_Particle = GetComponent<ParticleSystem>();
        toIgnore = new List<GameObject>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && !toIgnore.Contains(other))
        {
            toIgnore.Add(other);

            Stats m_Stats = other.GetComponent<Stats>();
            if (m_Stats)
            {
                float dist = Vector2.Distance(transform.position, other.transform.position);
                dist = Mathf.Clamp(dist, 1.0f, 999f);
                float dmg = -m_BaseDamage / dist;
                m_Stats.ModHealth((int)dmg);
            }
        }
    }
}
