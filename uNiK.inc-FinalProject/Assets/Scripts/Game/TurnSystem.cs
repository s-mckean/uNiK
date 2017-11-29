using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    [SerializeField] private List<TeamHandler> m_Teams;

    private TankController[] m_ActiveTeamControllers;
    private Stats[] m_ActiveTeamStats;
    private TankController m_ActiveCharacter;
    private TeamHandler m_ActiveTeam;
    private int m_ActiveTeamIndex;
    private int m_ActiveCharacterIndex;
    private Camera m_ActiveCamera;

    public static TurnSystem Instance;

	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        m_ActiveTeamIndex = 0;
        m_ActiveCharacterIndex = 0;

        Invoke("Setup", 1f);
	}

    private void Setup()
    {
        foreach (TeamHandler team in m_Teams)
        {
            foreach (TankController controller in team.GetTeamControllers())
            {
                ActivateCharacter(controller, false);
                IgnoreCollisionsWithOtherPlayers(controller.gameObject);
            }
        }

        m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
        m_ActiveTeamControllers = m_ActiveTeam.GetTeamControllers();
        m_ActiveTeamStats = m_ActiveTeam.GetTeamStats();
        m_ActiveCharacter = m_ActiveTeamControllers[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);
    }

    private void IgnoreCollisionsWithOtherPlayers(GameObject player)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        foreach (TeamHandler team in m_Teams)
        {
            foreach (TankController controller in team.GetTeamControllers())
            {
                Physics2D.IgnoreCollision(playerCollider, controller.gameObject.GetComponent<Collider2D>());
            }
        }
    }

    public void NextTurn()
    {
        ActivateCharacter(m_ActiveCharacter, false);

        do
        {
            NextCharacter();
        } while (!m_ActiveTeamStats[m_ActiveCharacterIndex].isAlive());

        m_ActiveCharacter = m_ActiveTeamControllers[m_ActiveCharacterIndex];
        ActivateCharacter(m_ActiveCharacter, true);
    }

    private void NextCharacter()
    {
        if (m_ActiveCharacterIndex + 1 < m_ActiveTeam.GetTeamControllers().Length)
        {
            m_ActiveCharacterIndex++;
        }
        else
        {
            do
            {
                m_ActiveTeamIndex = (m_ActiveTeamIndex + 1) % m_Teams.Count;
                m_ActiveTeam = m_Teams[m_ActiveTeamIndex];
                m_ActiveTeamControllers = m_ActiveTeam.GetTeamControllers();
                m_ActiveTeamStats = m_ActiveTeam.GetTeamStats();
                m_ActiveCharacterIndex = 0;
            } while (m_ActiveTeamControllers.Length <= 0);
        }
    }

    private void ActivateCharacter(TankController tankController, bool active)
    {
        if (active)
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 5;
            m_ActiveCamera = tankController.gameObject.GetComponentInChildren<Camera>();
            tankController.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        } 
        else
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 0;
            tankController.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        ActivateTankControls(tankController, active);
    }

    private void ActivateTankControls(TankController tankController, bool active)
    {
        tankController.IsActive = active;
        tankController.GetComponentInChildren<GameCharacter>().ActivateCharacter(active);

        // Temporary fix so you can't move other tanks by moving into them
        //tankController.gameObject.GetComponent<Rigidbody2D>().isKinematic = !active;

        var weapSys = tankController.GetComponentInChildren<WeaponSystem>();
        if (weapSys != null)
        {
            weapSys.ActivateSystem(active);
        }
    }

    public void ShotFired(GameObject projectile)
    {
        ActivateTankControls(m_ActiveCharacter, false);
        StartCoroutine(AdjustCamera(projectile));
    }

    private IEnumerator AdjustCamera(GameObject projectile)
    {
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        float origOrthoSize = m_ActiveCamera.orthographicSize;
        float maxHeight = rb.position.y;

        while (projectile != null && rb.velocity != Vector2.zero)
        {
            if (projectile.GetComponent<SpriteRenderer>().enabled)
            {
                m_ActiveCamera.GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x * 1.25f, 0);
            }
            else
            {
                m_ActiveCamera.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (rb.velocity.y > 0 && rb.position.y >= maxHeight)
            {
                float newOrthoSize = m_ActiveCamera.orthographicSize + Mathf.Abs(rb.velocity.y) / 100f;
                m_ActiveCamera.orthographicSize = Mathf.Clamp(newOrthoSize, origOrthoSize, 8f);
                maxHeight = rb.position.y;
            }

            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(2.0f);

        m_ActiveCamera.orthographicSize = origOrthoSize;
        m_ActiveCamera.transform.localPosition = new Vector3(0, 0, -10);
        ActivateTankControls(m_ActiveCharacter, true);
    }
}
