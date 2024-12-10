using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines; // Import the namespace for VolumetricLineBehavior

public class LaserProjectile : MonoBehaviour
{
    public float speed = 20f; // Speed of the laser
    public float lifetime = 2f; // Time before the laser is destroyed
    public float maxDistance = 50f; // Maximum distance the laser travels
    public GameObject hitEffectPrefab; // Prefab for the hit effect

    private VolumetricLineBehavior laserBehavior; // Reference to the VolumetricLineBehavior
    private Vector3 startPosition;

    void Start()
    {
        laserBehavior = GetComponent<VolumetricLineBehavior>();
        startPosition = transform.position;

        // Set the initial positions for the laser beam
        if (laserBehavior != null)
        {
            laserBehavior.StartPos = Vector3.zero; // Relative to the projectile
            laserBehavior.EndPos = Vector3.forward * maxDistance; // Length of the beam
        }

        // Destroy the laser after its lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the laser forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update the beam's end position to simulate movement
        if (laserBehavior != null)
        {
            float traveledDistance = Vector3.Distance(startPosition, transform.position);
            laserBehavior.EndPos = Vector3.forward * Mathf.Max(0, maxDistance - traveledDistance);
        }

        // Destroy the laser if it exceeds maxDistance
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Spawn hit effect at the point of impact
        SpawnHitEffect(collision);

        // Destroy the laser projectile
        Destroy(gameObject);

        // Optional: Additional logic for specific cases
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit an enemy!");
            // Add logic to damage the enemy, if applicable
        }
    }

    private void SpawnHitEffect(Collision collision)
    {
        if (hitEffectPrefab != null)
        {
            // Get the point of collision
            ContactPoint contact = collision.contacts[0];

            // Instantiate the hit effect at the collision point
            GameObject hitEffect = Instantiate(hitEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));

            // Optional: Destroy the hit effect after a delay to prevent clutter
            Destroy(hitEffect, 2f);
        }
        else
        {
            Debug.LogWarning("Hit effect prefab is not assigned!");
        }
    }
}
