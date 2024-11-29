using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Tower : MonoBehaviour
{
    public GameObject magicBulletPrefab;    //bullet prefab
    public Transform firePoint; //bullet spawn
    public LayerMask enemyLayer;    //enemy layer
    public Transform target;    //current target

    [Header("Tower Behavior")]
    public float range  = 10f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    // Update is called once per frame
    void Update()
    {
        //locate target
        UpdateTarget();

        //Rotate toward the target
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f && target != null)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach(Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(magicBulletPrefab, firePoint.position, firePoint.rotation);
        Magic_Bullet bullet = projectile.GetComponent<Magic_Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
