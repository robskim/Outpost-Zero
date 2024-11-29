using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M10_Tower : MonoBehaviour
{
    public Transform target;    //current target
    public GameObject bulletPrefab; //bullet prefab
    public Transform firePoint; //point where bullet spawn
    public LayerMask enemyLayer; //layer mask for enemies

    [Header("Tower Behavior")]
    public float range = 15f;   //tower range
    public float fireRate = 0.3f;   //the lower fireRate, the slower firing bullet
    public float fireCountdown = 0f;    //delay

    // Update is called once per frame
    void Update()
    {
        //Find enemy
        UpdateTarget();

        //Rotate toward the target
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        //check if the tower can fire
        if (target != null && fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        //cooldown
        fireCountdown -= Time.deltaTime;
    }

    //Locate target
    void UpdateTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range, enemyLayer);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    //Tower shoot bullet
    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        M10_Bullet bullet = bulletGO.GetComponent<M10_Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
