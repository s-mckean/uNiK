using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    [SerializeField] private List<TeamHandler> m_Teams;
    [SerializeField] private Camera m_ProjectileCamera;

    private TankController[] m_ActiveTeamControllers;
    private Stats[] m_ActiveTeamStats;
    private TankController m_ActiveCharacter;
    private TeamHandler m_ActiveTeam;
    private int m_ActiveTeamIndex;
    private int m_ActiveCharacterIndex;

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
            if (CheckAllDead())
            {
                return;
            }
            NextCharacter();
        } while (!m_ActiveTeamStats[m_ActiveCharacterIndex].IsAlive());

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

    private bool CheckAllDead()
    {
        foreach (TeamHandler team in m_Teams)
        {
            foreach (Stats stats in team.GetTeamStats())
            {
                if (stats.IsAlive())
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ActivateCharacter(TankController tankController, bool active)
    {
        if (active)
        {
            tankController.gameObject.GetComponentInChildren<Camera>().depth = 5;
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
        if (m_ProjectileCamera != null)
        {
            m_ProjectileCamera.GetComponent<Transform>().SetPositionAndRotation(
                m_ActiveCharacter.gameObject.GetComponentInChildren<Camera>().GetComponent<Transform>().position,
                m_ActiveCharacter.gameObject.GetComponentInChildren<Camera>().GetComponent<Transform>().rotation);
        }
        else
        {
            ActivateTankControls(m_ActiveCharacter, true);
            yield break;
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        m_ProjectileCamera.depth = 10;
        Rigidbody2D camBdy = m_ProjectileCamera.GetComponent<Rigidbody2D>();
        float origOrthoSize = m_ProjectileCamera.orthographicSize;
        float maxHeight = rb.position.y;

        while (projectile != null && rb.velocity != Vector2.zero)
        {
            if ((projectile.GetComponent<SpriteRenderer>() != null &&
                projectile.GetComponent<SpriteRenderer>().enabled) ||
                (projectile.GetComponent<ParticleSystem>() != null &&
                projectile.GetComponent<ParticleSystem>().isEmitting))
            {
                camBdy.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.75f);
            }
            else
            {
                camBdy.velocity = Vector2.zero;
            }

            if (rb.velocity.y > 0 && rb.position.y >= maxHeight)
            {
                float newOrthoSize = m_ProjectileCamera.orthographicSize + Mathf.Abs(rb.velocity.y) / 100f;
                m_ProjectileCamera.orthographicSize = Mathf.Clamp(newOrthoSize, origOrthoSize, 8f);
                maxHeight = rb.position.y;
            }

            yield return new WaitForFixedUpdate();
        }

        camBdy.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.0f);

        m_ProjectileCamera.depth = 0;
        NextTurn();
    }

    public List<TeamHandler> GetTeams()
    {
        return this.m_Teams;
    }
}
