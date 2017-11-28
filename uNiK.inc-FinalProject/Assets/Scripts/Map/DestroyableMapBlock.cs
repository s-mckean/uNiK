﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableMapBlock : MonoBehaviour {

    [SerializeField] private GameObject particleEffects;

    private GameObject spawnedParticleEffects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) {
            DestroyBlock();
        }
    }

    public void DestroyBlock()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        spawnedParticleEffects = GameObject.Instantiate(particleEffects,
            this.gameObject.GetComponent<Transform>());
        ParticleSystem particleSys = spawnedParticleEffects.GetComponent<ParticleSystem>();
        var particleSysMain = particleSys.main;
        particleSysMain.startColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        Destroy(spawnedParticleEffects, particleSysMain.duration);
        Destroy(this.gameObject, particleSysMain.duration);
    }
}