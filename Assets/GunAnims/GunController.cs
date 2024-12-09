using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Animator anim;
    bool Running = false;
    bool Shooting = false;

    public GameObject laserPrefab; // Laser projectile prefab
    public Transform muzzle; // Muzzle position
    public ParticleSystem smokeParticles; // Smoke particle system

    public float fireRate = 0.1f; // Time between shots
    public float heatPerShot = 5f; // Heat added per shot
    public float heatDecayRate = 2.5f; // Heat decrease per second
    public float maxHeat = 250f; // Maximum heat level before overheating
    private float currentHeat = 0f; // Current heat level
    private float nextFireTime = 0f;
    private bool overheated = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (smokeParticles != null)
        {
            smokeParticles.Stop(); // Ensure the particle system is off at the start
        }
    }

    void Update()
    {
        // Check if the player is walking
        bool Walking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        anim.SetBool("Walking", Walking);

        // Decay heat over time when not overheated
        if (!overheated)
        {
            currentHeat = Mathf.Max(0, currentHeat - heatDecayRate * Time.deltaTime);
        }

        // Handle shooting
        HandleShooting();

        // Handle running if not shooting
        if (Walking && !Shooting && !overheated)
        {
            HandleRunning();
        }
        else
        {
            StopRunning();
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButton(0) && !overheated)
        {
            // Shooting starts or continues
            if (!Shooting)
            {
                Shooting = true;
                anim.SetBool("Shooting", true);

                // Stop running if currently running
                if (Running)
                {
                    Running = false;
                    anim.SetBool("RunningMid", false);
                    anim.ResetTrigger("RunningStart");
                    anim.SetTrigger("RunningEnd");
                }
            }

            // Fire laser based on fireRate
            if (Time.time >= nextFireTime)
            {
                FireLaser();
            }
        }
        else
        {
            // Stop shooting only if the left mouse button is released
            if (Shooting)
            {
                Shooting = false;
                anim.SetBool("Shooting", false);
            }
        }
    }

    void HandleRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!Running)
            {
                anim.SetTrigger("RunningStart");
            }

            Running = true;
            anim.SetBool("RunningMid", true);
        }
        else if (Running)
        {
            StopRunning();
        }
    }

    void StopRunning()
    {
        if (Running)
        {
            Running = false;
            anim.SetBool("RunningMid", false);
            anim.SetTrigger("RunningEnd");
        }
    }

    void FireLaser()
    {
        nextFireTime = Time.time + fireRate;

        // Add heat per shot
        currentHeat += heatPerShot;

        // Check for overheating
        if (currentHeat >= maxHeat)
        {
            TriggerOverheat();
            return;
        }

        // Instantiate the laser projectile
        if (laserPrefab != null && muzzle != null)
        {
            Instantiate(laserPrefab, muzzle.position, muzzle.rotation);
        }

        Debug.Log($"Laser Fired! Current Heat: {currentHeat}");
    }

    void TriggerOverheat()
    {
        overheated = true;
        Shooting = false; // Stop shooting when overheating
        anim.SetBool("Shooting", false);
        anim.SetTrigger("Overheat"); // Play the overheat animation
        Debug.Log("Weapon Overheated!");
        StartCoroutine(OverheatCooldown());
    }

    IEnumerator OverheatCooldown()
    {
        // Allow the overheat animation to play completely
        yield return new WaitForSeconds(3f);

        // Reset heat and re-enable functionality
        currentHeat = 0f;
        overheated = false;
        Debug.Log("Weapon Cooled Down!");
    }

    // Animation Event: Activate smoke particles
    public void ActivateSmoke()
    {
        if (smokeParticles != null)
        {
            smokeParticles.Play();
            Debug.Log("Smoke Activated");
        }
    }

    // Animation Event: Deactivate smoke particles
    public void DeactivateSmoke()
    {
        if (smokeParticles != null)
        {
            smokeParticles.Stop();
            Debug.Log("Smoke Deactivated");
        }
    }
}
