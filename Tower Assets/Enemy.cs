using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public float speed = 5f;
    private float originalSpeed;
    private bool isSlowed = false;

    private void Start()
    {
        originalSpeed = speed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyStatusEffect(string effectType, float duration)
    {
        switch (effectType)
        {
            case "Burn":
                StartCoroutine(BurnEffect(duration));
                break;
            case "Poison":
                StartCoroutine(PoisonEffect(duration));
                break;
            case "Slow":
                if (!isSlowed)
                {
                    isSlowed = true;
                    speed = originalSpeed * (1f - 0.5f);  //Reduce speed
                    StartCoroutine(SlowEffect(duration));
                }
                break;
        }
    }

    IEnumerator BurnEffect(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(5);
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PoisonEffect(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(3);
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SlowEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isSlowed = false;
    }
}
