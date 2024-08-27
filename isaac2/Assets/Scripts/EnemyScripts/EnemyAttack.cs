using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
///     Enemy Attack logic, inherited by EnemyMeleeAttack & EnemyRangedAttack
///     Implements:
///     - maxAttackDistance: furthest away Enemy will begin Attacking)
///     - attackCooldown: time between Attacks
///     - AttemptAttack(): every 0.2 seconds, check if Player is in Attack range,
///         and Enemy is Alert & not cooling down & not Dead, then call Attack()
/// </summary>
public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float maxAttackDistance;
    [SerializeField] private float attackCooldown;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Start attack coroutine
        StartCoroutine(AttemptAttack());
    }

    private IEnumerator AttemptAttack()
    {
        WaitForSeconds frameWait = new(0.2f);
        WaitForSeconds cooldownWait = new(this.attackCooldown);

        while (!this.GetComponent<EnemyHealth>().IsDead())
        {
            yield return frameWait;
            if (this.GetComponent<EnemyDetectPlayer>().IsAlert &&
                Vector2.Distance(transform.position, player.transform.position) <= this.maxAttackDistance)
            {
                Attack();
                yield return cooldownWait;
            }
        }
    }

    protected abstract void Attack();
}
