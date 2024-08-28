using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
///     Enemy Melee Attack logic.
///     Inherits from EnemyAttack, which implements:
///     - maxAttackDistance: furthest away Enemy will begin Attacking)
///     - attackCooldown: time between Attacks
///     - AttackDamage: base Damage dealt by Attack
///     - AttemptAttack(): every 0.2 seconds, check if Player is in Attack range,
///         and Enemy is Alert & not cooling down & not Dead, then call Attack()
/// </summary>
public class EnemyMeleeAttack : EnemyAttack
{
    #region Instance Variables
    #endregion
    
    #region Overridden Methods
    /// <summary>
    ///     Initiate Melee Attack.
    ///     Called when Enemy is within range, not cooling down, Alert, not Dead, etc.
    /// </summary>
    protected override void Attack()
    {
        Debug.Log("Melee Attack!");
        // Enemy Attack animation
        // Draw hitbox around Enemy weapon/arm/whatever each frame?
        // If intersects with Player, deal damage
        // Much more logic needed if each indv Enemy can do multiple types of Attacks

        // Temporary Attack:
        
    }
    #endregion

    #region Methods
    #endregion
}
