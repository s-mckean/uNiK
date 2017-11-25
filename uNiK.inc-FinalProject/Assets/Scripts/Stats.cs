using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int maxHealth = 100;       // Feel free to adjust the settings
    public int maxScore = 99999;

    private int score;
    private int health;
    private bool alive;

	// Use this for initialization
	void Start () {
        score = 0;
        health = maxHealth;
        alive = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetHealth()
    {
        return health;
    }

    public bool isAlive()
    {
        return alive;
    }

    public void ModHealth(int value)
    {
        health += value;
        CheckOverHealth();
        CheckDead();
    }

    private void CheckOverHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDead()
    {
        if (health < 1)
        {
            health = 0;
            alive = false;
        }
    }

    public void ModScore(int value)
    {
        score += value;
        CheckMinScore();
        CheckMaxScore();
    }

    private void CheckMinScore()
    {
        if (score < 0)
        {
            score = 0;
        }
    }

    private void CheckMaxScore()
    {
        if (score > maxScore)
        {
            score = maxScore;
        }
    }
}
