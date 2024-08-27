using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
///     Enemy Melee Attack logic.
///     Inherits from EnemyAttack, which implements:
///     - maxAttackDistance: furthest away Enemy will begin Attacking)
///     - attackCooldown: time between Attacks
///     - AttemptAttack(): every 0.2 seconds, check if Player is in Attack range,
///         and Enemy is Alert & not cooling down & not Dead, then call Attack()
/// </summary>
public class EnemyMeleeAttack : EnemyAttack
{
    
    /// <summary>
    ///     Initiate melee attack.
    ///     Called when Enemy is within range, not cooling down, alert, not dead, etc.
    /// </summary>
    protected override void Attack()
    {
        Debug.Log("Melee Attack!");
    }
}
