using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
///     Enemy Attack logic, inherited by EnemyMeleeAttack & EnemyRangedAttack
///     Implements:
///     - maxAttackDistance: furthest away Enemy will begin Attacking)
///     - attackCooldown: time between Attacks
///     - AttackDamage: base Damage dealt by Attack
///     - AttemptAttack(): every 0.2 seconds, check if Player is in Attack range,
///         and Enemy is Alert & not cooling down & not Dead, then call Attack()
/// </summary>
public abstract class EnemyAttack : MonoBehaviour
{
    #region Instance Variables
    private GameObject player;
    [SerializeField] private float maxAttackDistance;
    [SerializeField] private float attackCooldown;
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public float AttackKnockback { get; private set; }
    // Attack duration?
    #endregion

    #region Implemented Methods
    /// <summary>
    ///     Start
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Start Attack coroutine
        StartCoroutine(AttemptAttack());
    }

    /// <summary>
    ///     Attempt to initiate an Attack, if Player is within Attack range,
    ///     Enemy is not cooling down, and Enemy is Alert & not Dead.
    /// </summary>
    /// <returns></returns>
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
    #endregion

    #region Abstract Methods
    /// <summary>
    ///     Child must implement some sort of Attack
    /// </summary>
    protected abstract void Attack();
    #endregion
}
