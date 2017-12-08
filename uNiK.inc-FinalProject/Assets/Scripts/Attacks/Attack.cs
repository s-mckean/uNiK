using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    [SerializeField] protected float lifetime;
    [SerializeField] protected float initialDelay;
    [SerializeField] protected int baseDamage;
   

	// Use this for initialization
	void Start () {
       
        StartEvent();
        Effect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Impact(collision);
    }

    // What happens when the projectile is launched; like setting a time when it will be destroyed
    public virtual void StartEvent()
    {

    }

    // Visuals/Audio; activating particle systems or any sound effects when launched
    public virtual void Effect()
    {

    }

    // What happens when the projectile hits something
    public virtual void Impact(Collider2D collision)
    {

    }
}
