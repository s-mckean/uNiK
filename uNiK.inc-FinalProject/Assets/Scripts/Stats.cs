using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    [SerializeField] private GameObject explosion;

    public int maxHealth = 100;       // Feel free to adjust the settings
    public int maxScore = 99999;
    public float origTankSpeed = 12f;
    public Text dmgPopup;
    public int dmgPopupIterations = 5;
    public float iterationDelay = 0.1f;

    private int score;
    private int health;
    private bool alive;

	// Use this for initialization
	void Start () {
        score = 0;
        health = maxHealth;
        alive = true;
        ResetTankSpeed();
	}

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public void ModName(string name)
    {
        this.name = name;
    }

    public void ModHealth(int value)
    {
        health += value;
        StartCoroutine(PopupDamage(value));
        CheckOverHealth();
        CheckDead();
    }
    
    public void ModScore(int value)
    {
        score += value;
        CheckMinScore();
        CheckMaxScore();
    }

    public void ModTankSpeed(float speed)
    {
        GetComponent<TankController>().ModTankSpeed(speed);
    }

    public void ResetTankSpeed()
    {
        ModTankSpeed(origTankSpeed);
    }

    private IEnumerator PopupDamage(int value)
    {
        dmgPopup.text = "";
        int displayedValue = 0;
        int targetValue = Mathf.Abs(value);
        int remainder = targetValue % dmgPopupIterations;

        if (value < 0)
        {
            dmgPopup.color = Color.red;
        }
        else if (value > 0)
        {
            dmgPopup.color = Color.green;
        }

        while (displayedValue < Mathf.Abs(value))
        {
            displayedValue += (targetValue / dmgPopupIterations);
            if ((targetValue - displayedValue) == remainder) {
                displayedValue += remainder;
                remainder = 0;
            }
            dmgPopup.text = displayedValue.ToString();
            yield return new WaitForSeconds(iterationDelay);
        }

        yield return new WaitForSeconds(2.0f);

        dmgPopup.text = "";
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
            Die();
        }
    }

    private void Die()
    {
        alive = false;
        //GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<Rigidbody2D>().isKinematic = true;
        //GetComponent<Collider2D>().enabled = false;
        //Destroy(this.gameObject);
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = false;
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
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
