﻿using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
    [HideInInspector] public bool simulatedInput = false;
    [HideInInspector] public bool simulatedFireDown = false;
    [HideInInspector] public bool simulatedFire = false;
    [HideInInspector] public bool simulatedFireUp = false;
    [HideInInspector] public Rigidbody activeShell;

    private string m_FireButton;                // The input axis that is used for launching shells.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.

    private int m_firedShellsDuringSession = 0;
    public int firedShellsDuringSession 
    {
        get {
            return m_firedShellsDuringSession;
        }
    }

    public bool Fired 
    {
        get {
            return m_Fired;
        }
    }

    public float GetChargeSpeed 
    {
        get {
            return (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }
    }

    public string GetFireButtonName
    {
        get {
            return "Fire" + m_PlayerNumber;
        }
    }

    public string FireButton {
        get {
            return m_FireButton;
        }
    }

    public float ChargeSpeed {
        get {
            return m_ChargeSpeed;
        }
    }
    
    public float CurrentLaunchForce {
        get {
            return m_CurrentLaunchForce;
        }
    }
    
    public float AimSliderValue {
        get {
            return m_AimSlider != null 
                ? m_AimSlider.value
                : -1f;
        }
    }

    public void OnEnable()
    {
        // When the tank is turned on, reset the launch force and the UI
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    public void Start ()
    {
        // The fire axis is based on the player number.
        m_FireButton = GetFireButtonName;

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = GetChargeSpeed;
    }

    private void Update ()
    {
        HandleFiring();
    }

    private void HandleFiring()
    {
        // The slider should have a default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... use the max force and launch the shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            activeShell = Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
        else if (GetFireButtonDown())
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
        else if (GetFireButton() && !m_Fired)
        {
            // Increment the launch force and update the slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
        else if (GetFireButtonUp() && !m_Fired)
        {
            // ... launch the shell.
            activeShell = Fire();
        }
    }

    private bool GetFireButtonDown()
    {
        return simulatedInput 
            ? simulatedFireDown
            : Input.GetButtonDown(m_FireButton);
    }

    private bool GetFireButton()
    {
        return simulatedInput 
            ? simulatedFire
            : Input.GetButton(m_FireButton);
    }

    private bool GetFireButtonUp()
    {
        return simulatedInput 
            ? simulatedFireUp
            : Input.GetButtonUp(m_FireButton);
    }

    private Rigidbody Fire ()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;
        m_firedShellsDuringSession++;

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;
        return shellInstance;
    }
}