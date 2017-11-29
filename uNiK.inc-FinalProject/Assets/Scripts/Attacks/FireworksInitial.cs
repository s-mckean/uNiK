using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksInitial : MonoBehaviour {

    [SerializeField] private GameObject fireworksObj;
    [SerializeField] private float duration;
    [SerializeField] private bool isSprite;

    private void Awake()
    {
        Invoke("SpawnFireworks", duration);
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
        
        GameObject.Instantiate(fireworksObj, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 2.0f);
    }
}
