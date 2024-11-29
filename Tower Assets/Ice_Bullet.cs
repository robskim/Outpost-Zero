using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Bullet : MonoBehaviour
{
    private Transform target;

    [Header("Bullet Behavior")]
    public float speed = 7f;
    public int damage = 5;
    public float slowDuration = 3f; //how long slow effect lasts

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy (gameObject);
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
        Damage(target);
        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        Enemy enemyGO = enemy.GetComponent<Enemy>();
        if (enemyGO != null)
        {
            enemyGO.TakeDamage(damage);

            //Aplly slow effect
            enemyGO.ApplyStatusEffect("Slow", slowDuration);
        }
    }
}
