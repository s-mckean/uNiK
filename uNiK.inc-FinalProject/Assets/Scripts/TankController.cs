using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class TankController : MonoBehaviour {

    [SerializeField] private float m_Speed = 12f;
    [SerializeField] private float m_FuelMax = 100f;
    [SerializeField] private float m_JumpAccelerationMax = 15f;
    [SerializeField] private float m_JumpAcceleration = 1f;
    [SerializeField] private bool m_UnlimitedFuel = true;
    [SerializeField] private ParticleSystem m_ThrustersObject;
    [SerializeField] private Transform m_CrosshairPosition;

    private AudioSource thrusters; 

    private Rigidbody2D m_Rigidbody;
    private Transform m_Transform;
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;
    private float m_MovementInputValue;
    private float m_JumpInputValue;
    private string m_TurnAxisName;
    private float m_FuelCurrent;
    private bool m_IsActive;
    private float m_OrigCamOrthoSize;
    private Camera m_Camera;

    public SpriteRenderer c_SpriteRenderer;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Camera = GetComponentInChildren<Camera>();
        m_OrigCamOrthoSize = m_Camera.orthographicSize;
      
        thrusters = GetComponent<AudioSource>();

        m_IsActive = false;
        var em = m_ThrustersObject.emission;
        em.enabled = false;
        m_Animator.speed = 0;
    }

    private void Awake()
    {
        Invoke("ResetStatus", 1f);
    }

    private void ResetStatus()
    {
        m_JumpInputValue = 0;
        m_MovementInputValue = 0f;
        m_IsActive = true;
    }

    void OnEnable()
    {
        ResetStatus();
    }


    void OnDisable()
    {
        ActivateThrusters(false);
    }

    void Update()
    {
        if (m_IsActive)
        {
            m_MovementInputValue = Input.GetAxis("Horizontal");
            m_Animator.speed = Mathf.Abs(m_MovementInputValue);
            AdjustJumpValue();

            float newOrthoSize = m_Camera.orthographicSize + 15*Input.GetAxis("Mouse ScrollWheel");
            newOrthoSize = Mathf.Clamp(newOrthoSize, m_OrigCamOrthoSize, 40f);
            m_Camera.orthographicSize = newOrthoSize;
        }
    }

    private void AdjustJumpValue()
    {
        bool spaceDown = Input.GetKey(KeyCode.Space);
       
        if (spaceDown)
        {
            m_JumpInputValue = m_JumpAcceleration;        // Fixes the way falling and gravity works
            thrusters.Play();
        }
        else
        {
            m_JumpInputValue = 0;
        }

        ActivateThrusters(spaceDown);
        m_JumpInputValue = Mathf.Clamp(m_JumpInputValue, 0, m_JumpAccelerationMax);
    }

    private void ActivateThrusters (bool activate)
    {
        
        var em = m_ThrustersObject.emission;
        
        if (activate && CheckFuel())
        {
            em.enabled = true;
            
        }
        else
        {
            em.enabled = false;
            thrusters.Stop();
        }
        
    }

    void FixedUpdate()
    {
        if (CheckFuel() && m_IsActive)
        {
            Move();
            Turn();
            Jump();
            UseFuel();
        }
    }

    private void Jump()
    {
        
        float yVel = m_Rigidbody.velocity.y + m_JumpInputValue;       // Fixes the way falling and gravity works
        if (yVel > m_JumpAccelerationMax)
        {
            yVel = m_JumpAccelerationMax;
        }

        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, yVel);
    }


    private void Move()
    {
        m_Rigidbody.velocity = new Vector2(m_MovementInputValue * m_Speed, m_Rigidbody.velocity.y);
    }

    private void Turn()
    {
        if (m_CrosshairPosition == null)
        {
            return;
        }

        if (m_CrosshairPosition.position.x - m_Transform.position.x > 0)
        {
            m_SpriteRenderer.flipX = true;
            c_SpriteRenderer.flipX = true;
        }
        else if (m_CrosshairPosition.position.x - m_Transform.position.x < 0)
        {
            m_SpriteRenderer.flipX = false;
            c_SpriteRenderer.flipX = false;
        }
    }

    private void UseFuel()
    {
        if (!m_UnlimitedFuel)
        {
            m_FuelCurrent -= Mathf.Abs(m_MovementInputValue) / 2f;
            m_FuelCurrent -= Mathf.Abs(m_JumpInputValue);
        }
    }

    private bool CheckFuel()
    {
        if (m_FuelCurrent <= 0)
        {
            m_FuelCurrent = 0;
        }

        return m_FuelCurrent > 0;
    }

    public bool IsActive
    {
        get
        {
            return m_IsActive;
        }
        set
        {
            m_IsActive = value;

            if (!m_IsActive) {
                m_Camera.orthographicSize = m_OrigCamOrthoSize;
                ActivateThrusters(false);
            }
            else
            {
                ResetStatus();
            }
        }
    }

    public float GetCurrentFuel()
    {
        return m_FuelCurrent;
    }

    public void ModTankSpeed(float speed)
    {
        m_Speed = speed;
    }

    public void AddFuel(float fuel)
    {
        m_FuelCurrent += fuel;
    }

    public bool UnlimitedFuel
    {
        get
        {
            return this.m_UnlimitedFuel;
        }
        set
        {
            this.m_UnlimitedFuel = value;
        }
    }

    public void ReFuel()
    {
        m_FuelCurrent = m_FuelMax;
    }
}
