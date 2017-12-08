using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    [SerializeField] private GameObject explosion;

    public int maxHealth = 100;       // Feel free to adjust the settings
    public int maxScore = 99999;
    public float origTankSpeed = 12f;
    public int points = 1000;
    public Text dmgPopup;
    [Tooltip("Total delay is number of iterations x iteration delay")]
    public int dmgPopupIterations = 5;
    [Tooltip("Used for the 1st function for damage popping up")]
    public float iterationDelay = 0.1f;
    [Tooltip("Used for the 2nd function for damage popping up")]
    public float totalDelay = 0.5f;

    private Transform dmgPopupPos;
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

    public void SetAlive(bool alive)
    {
        this.alive = alive;
    }

    public void ModName(string name)
    {
        this.name = name;
    }

    public void ModHealth(int value)
    {
        health += value;
        StartCoroutine(PopupDamage2(value));
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
        dmgPopupPos = dmgPopup.transform;
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
            //dmgPopup.transform.position = (dmgPopupPos.position + new Vector3(0, 0.1f, 0));
            yield return new WaitForSeconds(iterationDelay);
        }

        yield return new WaitForSeconds(2.0f);

        dmgPopup.text = "";
    }

    private IEnumerator PopupDamage2(int value)
    {
        dmgPopup.text = "";
        int displayedValue = 0;
        int targetValue = Mathf.Abs(value);

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
            displayedValue++;
            dmgPopup.text = displayedValue.ToString();
            //dmgPopup.transform.position = (dmgPopupPos.position + new Vector3(0, 0.1f, 0));
            yield return new WaitForSeconds(iterationDelay / targetValue);
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
        GetComponent<Rigidbody2D>().isKinematic = true;
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
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = false;
        }

        if (GetComponent<TankController>() && GetComponent<TankController>().enabled)
        {
            Invoke("Event_DieOnTurn", 2.0f);
            GetComponent<TankController>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        
        GetComponentInChildren<GenericAim>().enabled = false;
        GetComponentInChildren<WeaponSystem>().enabled = false;
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);

    }

    private void Event_DieOnTurn()
    {
        TurnSystem.Instance.NextTurn();
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
