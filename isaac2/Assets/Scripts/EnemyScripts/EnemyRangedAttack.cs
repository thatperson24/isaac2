using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Enemy Ranged Attack logic.
///     Inherits from EnemyAttack, which implements:
///     - maxAttackDistance: furthest away Enemy will begin Attacking)
///     - attackCooldown: time between Attacks
///     - AttackDamage: base Damage dealt by Attack
///     - AttemptAttack(): every 0.2 seconds, check if Player is in Attack range,
///         and Enemy is Alert & not cooling down & not Dead, then call Attack()
/// </summary>
public class EnemyRangedAttack : EnemyAttack
{
    #region Instance Variables
    #endregion

    #region Overridden Methods
    /// <summary>
    ///     Initiate Ranged Attack.
    ///     Called when Enemy is within range, not cooling down, Alert, not Dead, etc.
    /// </summary>
    protected override void Attack()
    {
        Debug.Log("Ranged Attack!");
    }
    #endregion

    #region Methods
    #endregion
}
