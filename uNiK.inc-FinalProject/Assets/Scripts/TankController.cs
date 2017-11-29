using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    [SerializeField] private float m_Speed = 12f;
    [SerializeField] private float m_FuelMax = 100f;
    [SerializeField] private float m_JumpAccelerationMax = 15f;
    [SerializeField] private float m_JumpAcceleration = 1f;
    [SerializeField] private bool m_UnlimitedFuel = true;
    [SerializeField] private ParticleSystem m_ThrustersObject;
    [SerializeField] private Transform m_CrosshairPosition;
    [SerializeField] private Stats stats;

    /*
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    */

    private Rigidbody2D m_Rigidbody;
    private Transform m_Transform;
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;
    private float m_MovementInputValue;
    private float m_JumpInputValue;
    private string m_TurnAxisName;
    private float m_FuelCurrent;
    private bool m_IsActive;

    /*     
    private float m_OriginalPitch;
    private string m_MovementAxisName;     
    private float m_TurnInputValue;   
    */

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_IsActive = false;
        var em = m_ThrustersObject.emission;
        em.enabled = false;
        m_Animator.speed = 0;
    }

    private void Awake()
    {
        

        Invoke("ResetStatus", 1f);
        /*
       m_OriginalPitch = m_MovementAudio.pitch;
       */
    }

    private void ResetStatus()
    {
        m_FuelCurrent = m_FuelMax;
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
        
    }

    void Update()
    {
        if (m_IsActive)
        {
            m_MovementInputValue = Input.GetAxis("Horizontal");
            m_Animator.speed = Mathf.Abs(m_MovementInputValue);
            AdjustJumpValue();
        }
        
        // EngineAudio();
    }

    /*
     * Tweaked from Unity's Tank Game's TankMovement.cs
     * ~natechica
     
    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(m_MovementInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else if (checkFuel())
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }

     */

    private void AdjustJumpValue()
    {
        bool spaceDown = Input.GetKey(KeyCode.Space);
        if (spaceDown)
        {
            m_JumpInputValue = m_JumpAcceleration;        // Fixes the way falling and gravity works
            //m_JumpInputValue += m_JumpAcceleration;
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
        //m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpInputValue);
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
        /*
        if (m_MovementInputValue > 0)
        {
            m_SpriteRenderer.flipX = true;
        }
        else if (m_MovementInputValue < 0)
        {
            m_SpriteRenderer.flipX = false;
        }
        */

        if (m_CrosshairPosition.position.x - m_Transform.position.x > 0)
        {
            m_SpriteRenderer.flipX = true;
        }
        else if (m_CrosshairPosition.position.x - m_Transform.position.x < 0)
        {
            m_SpriteRenderer.flipX = false;
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
}
