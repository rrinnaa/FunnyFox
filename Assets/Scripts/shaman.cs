using UnityEngine;
using System.Collections;

public class ShamanEnemy : MonoBehaviour
{
    public float hp = 350f;
    public enum Speed { Slow = 1 , Medium = 2, Fast = 3 }
    public Speed speed = Speed.Slow;
    public float freezeDamagePercentage = 0.02f;
    public float freezeDuration = 6f;
    public float freezeTickInterval = 1f;
    public float attackDamage = 50f;
    public float attackRange = 2f;
    public float movementSpeed = 5f;
    public GameObject deathEffectPrefab;

    private bool isFrozen = false;
    private bool isAttacking = false;
    private float freezeTimer = 0f;
    private float freezeDamageTimer = 0f;

    private void Update()
    {
        if (hp <= 0)
        {
            Die();
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MoveTowardsTarget(player.transform);
            CheckAttackRange(player.transform);
        }

        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            freezeDamageTimer -= Time.deltaTime;

            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                Debug.Log("Character unfrozen.");
            }

            if (freezeDamageTimer <= 0f)
            {
                DealFreezeDamage();
                freezeDamageTimer = freezeTickInterval;
            }
        }
    }

    private void MoveTowardsTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
    }

    private void CheckAttackRange(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        // Perform attack logic here
        Debug.Log("Attacking target.");
    }

    public void FreezeCharacter()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            freezeTimer = freezeDuration;
            Debug.Log("Character frozen.");
        }
    }

    private void DealFreezeDamage()
    {
        float damage = hp * freezeDamagePercentage;
        hp -= damage;
        Debug.Log("Freeze damage: " + damage);
    }

    private void Die()
    {
        Debug.Log("Character died.");
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}