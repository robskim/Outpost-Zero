using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 10;

    private Transform[] path;
    private int pathIndex = 0;

    void Start()
    {
        // Find the path (ensure you create a PathManager script or replace with actual waypoints)
        path = FindObjectOfType<PathManager>().GetPath();
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (pathIndex < path.Length)
        {
            // Get target waypoint
            Transform target = path[pathIndex];

            // Calculate the direction to the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Smoothly rotate to face the target
            if (direction.magnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Move towards the target
                transform.position += direction * speed * Time.deltaTime;
            }
            // Check if close enough to the target to move to the next waypoint
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
                pathIndex++;
        }
        else
        {
            ReachEnd();
        }
    }

    void ReachEnd()
    {
        Destroy(gameObject); // Replace with logic for when enemies reach the end
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy enemy on death
        }
    }

}

