using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlocks : MonoBehaviour {

    private float m_CollisionDisableTimer;

	// Use this for initialization
	void Start () {
        m_CollisionDisableTimer = GetComponent<ParticleSystem>().main.duration;
        m_CollisionDisableTimer /= 2;
	}

    private void OnParticleCollision(GameObject gObj)
    {
        if (gObj.CompareTag("DestroyableBlock"))
        {
            gObj.GetComponent<DestroyableMapBlock>().DestroyBlock();
            
            Invoke("DisableCollisionTriggers", m_CollisionDisableTimer);
        }
    }

    private void DisableCollisionTriggers()
    {
        var m_Collision = GetComponent<ParticleSystem>().collision;
        m_Collision.enabled = false;
    }
}
