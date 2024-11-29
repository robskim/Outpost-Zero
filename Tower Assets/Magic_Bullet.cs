using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Bullet : MonoBehaviour
{
    private Transform target;

    [Header("Bullet Behavior")]
    public float speed = 20f;
    public float explosionRadius = 3f;
    public int damage = 30;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceToFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceToFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceToFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] enemy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider colliders in enemy)
        {
            if (colliders.CompareTag("Enemy"))
            {
                Damage(colliders.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy enemyGO = enemy.GetComponent<Enemy>();
        if (enemyGO != null)
        {
            enemyGO.TakeDamage(damage);

            //Apply poison status
            enemyGO.ApplyStatusEffect("Poison", 5f);
        }
    }
}
