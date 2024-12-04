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
    public float fireRate = 0.1f; // Time between shots
    private float nextFireTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is walking
        bool Walking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        anim.SetBool("Walking", Walking);

        // Handle shooting
        HandleShooting();

        // Handle running if not shooting
        if (Walking && !Shooting)
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
        if (Input.GetMouseButton(0))
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

        // Instantiate the laser projectile
        if (laserPrefab != null && muzzle != null)
        {
            Instantiate(laserPrefab, muzzle.position, muzzle.rotation);
        }
    }
}
