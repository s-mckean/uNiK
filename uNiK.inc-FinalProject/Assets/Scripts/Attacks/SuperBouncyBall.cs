using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBouncyBall : MonoBehaviour {

    [SerializeField] private float m_Damage;
    [SerializeField] private float m_Duration;

    private void Awake()
    {
        Destroy(this.gameObject, m_Duration);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DestroyableBlock"))
        {
            collision.gameObject.GetComponent<DestroyableMapBlock>().DestroyBlock();
        }
    }

}
