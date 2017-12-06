using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksInitial : MonoBehaviour {

    [SerializeField] private GameObject fireworksObj;
    [SerializeField] private float duration;
    [SerializeField] private bool isSprite;
    [SerializeField] private Camera projectileCam;

    private void Awake()
    {
        Invoke("SpawnFireworks", duration);
        projectileCam = GameObject.FindGameObjectsWithTag("ProjectileCamera")[0].GetComponent<Camera>();
    }

    private void SpawnFireworks()
    {
        if (isSprite)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            this.gameObject.GetComponentInChildren<ParticleSystem>().Clear();
            this.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        }
        else
        {
            this.gameObject.GetComponent<ParticleSystem>().Clear();
            this.gameObject.GetComponent<ParticleSystem>().Stop();
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        
        GameObject fireworks = GameObject.Instantiate(fireworksObj, transform.position, Quaternion.identity);

        if (projectileCam)
        {
            TurnSystem.Instance.Event_ForceStopProjectileCamera();
            Vector2 vel = new Vector2(0, -27f);
            projectileCam.GetComponent<Rigidbody2D>().velocity = vel;
            projectileCam.orthographicSize = 20f;
            Invoke("NextTurn", 5.0f);
        }

        Destroy(this.gameObject, 6.0f);
    }

    private void NextTurn()
    {
        TurnSystem.Instance.NextTurn();
    }
}
