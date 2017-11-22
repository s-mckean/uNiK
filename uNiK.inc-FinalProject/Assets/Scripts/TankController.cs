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

    /*
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    */

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private Transform m_Transform;
    private Vector3 m_FacingLeftScale;
    private Vector3 m_FacingRightScale;
    private float m_MovementInputValue;
    private float m_JumpInputValue;
    private string m_TurnAxisName;
    private float m_CurrentYRotation;
    private bool isActive;
    private float m_FuelCurrent;

    /*     
    private float m_OriginalPitch;
    private string m_MovementAxisName;     
    private float m_TurnInputValue;   
    */


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
        m_FacingLeftScale = new Vector3(1, 1, 1);
        m_FacingRightScale = new Vector3(-1, 1, 1);
        isActive = false;
        ResetStatus();
        /*
       m_OriginalPitch = m_MovementAudio.pitch;
       */
    }

    private void ResetStatus()
    {
        m_FuelCurrent = m_FuelMax;
        m_JumpInputValue = 0;
        m_MovementInputValue = 0f;
        m_ThrustersObject.Pause();
    }

    void OnEnable()
    {
        ResetStatus();
        isActive = true;
    }


    void OnDisable()
    {
        isActive = false;
    }

    void Update()
    {
        m_MovementInputValue = Input.GetAxis("Horizontal");
        m_Animator.speed = Mathf.Abs(m_MovementInputValue);
        AdjustJumpValue();
        // EngineAudio();
        //Debug.Log("Fuel Left: " + fuelCurrent);
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
            m_JumpInputValue += m_JumpAcceleration;
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
        if (activate && CheckFuel())
        {
            m_ThrustersObject.Play();
        }
        else
        {
            m_ThrustersObject.Clear();
            m_ThrustersObject.Pause();
        }
    }

    void FixedUpdate()
    {
        if (CheckFuel())
        {
            Move();
            Turn();
            Jump();
            UseFuel();
        }
    }

    private void Jump()
    {
        Vector3 force = new Vector3(0, m_JumpInputValue, 0);
        m_Rigidbody.AddForce(force);
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 oldPos = m_Rigidbody.position;
        float newX = oldPos.x + m_MovementInputValue / m_Speed;
        Vector3 newPos = new Vector3(newX, oldPos.y, oldPos.z);
        m_Rigidbody.MovePosition(newPos);
    }

    private void Turn()
    {
        if (m_MovementInputValue > 0)
        {
            m_Transform.localScale = m_FacingRightScale;
        }
        else if (m_MovementInputValue < 0)
        {
            m_Transform.localScale = m_FacingLeftScale;
        }
    }

    private void UseFuel()
    {
        if (!m_UnlimitedFuel)
        {
            m_FuelCurrent -= Mathf.Abs(m_MovementInputValue) / 3f;
            m_FuelCurrent -= Mathf.Abs(m_JumpInputValue) / 60f;
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
}
