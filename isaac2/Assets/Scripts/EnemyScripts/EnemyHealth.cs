using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// Health class stores entity health and deals with setting, incrementing,
    /// and fetching both current and max health.
    /// Also detects death and overheal.
    /// In theory this was meant to be shared between Player and Enemy,
    /// but I am not sure if that is a good idea or not.
    /// </summary>

    // TODO: If we want health bars they could be handled here

    private const int MaxMaxHealth = 999;  // the max possible max health


    // Properties
    [Range(1, MaxMaxHealth)] [SerializeField] private int _maxHealth;
    public int MaxHealth
    { 
        get => this._maxHealth;
        private set => Math.Max(1, value);
    }
    [SerializeField] private int _curHealth;
    public int CurHealth
    { 
        get => this._curHealth;
        private set 
        {
            if (this.IsDead())
            {
                return;  // prevent healing of dead entities
            }
            else if (value > this.MaxHealth) // OVERHEAL
            {
                this._curHealth = this.MaxHealth;
                // TODO: overheal - could block use of heal item or convert heal to something else
            }
            else if (value <= 0) // DIED
            {
                this._curHealth = 0;
                EnemyDeath();
            }
            else
            {
                this._curHealth = value;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.CurHealth = this.MaxHealth;
    }

    // Update is called once per frame
    //void Update()
    //{
        // Could implement gradual natural healing over time I guess?
        // otherwise can we delete this method entirely (Unity question)
    //}




    /// <summary>
    ///     Increment current health by given +/- delta amount.
    ///     Calls setHealth which handles death, overheal, etc.
    ///     Delta is 1 by default (enemy heals by 1 HP).
    ///     Resists attemps to heal/damage a dead entity.
    ///     Returns new health in case caller needs.
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>health</returns>
    private int IncrementCurHealth(int delta = 1)
    {
        return this.CurHealth += delta;
    }

    /// <summary>
    ///     Decrement current health (damage) by given delta amount.
    ///     Delta is 1 by default (entity takes 1 HP damage).
    ///     Resists attemps to damage a dead entity.
    ///     Returns new health in case caller needs.
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>health</returns>
    public int Damage(int delta = 1)
    {
        return IncrementCurHealth(-delta);
    }

    /// <summary>
    ///     Increment current health (heal) by given delta amount.
    ///     Delta is 1 by default (entity heals 1 HP).
    ///     Resists attemps to heal a dead entity.
    ///     Returns new health in case caller needs.
    ///     Might want to return overheal flag?
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>health</returns>
    public int Heal(int delta = 1)
    {
        return IncrementCurHealth(delta);
    }

    /// <summary>
    ///     Set current health to the max health / fully heal entity.
    ///     Resists attemps to heal a dead entity.
    /// </summary>
    /// <returns>health</returns>
    public int ResetHealth()
    {
        return this.CurHealth = this.MaxHealth;
    }

    /// <summary>
    ///     Increment maxHealth by given delta amount.
    ///     Delta is 1 by default (entity gets 1 more max HP).
    ///     Could be used for powerups etc.
    ///     Returns new maxHealth in case caller needs.
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>maxHealth</returns>
    public int IncrementMaxHealth(int delta = 1) 
    {
        return this.MaxHealth += delta;
    }

    /// <summary>
    ///     Return whether or not entity is dead.
    /// </summary>
    /// <returns>isDead</returns>
    public bool IsDead()
    {
        return this.CurHealth <= 0;
    }

    /// <summary>
    ///     Initiate Enemy death process.
    /// </summary>
    public void EnemyDeath()
    {
        this.GetComponent<EnemyPursue>().SetCurSpeed(0);
        // TODO: unalert/stop detecting player?
        // TODO: remove entity
        // TODO: replace entity with "corpse" entity with no AI (or collision?)

    }
}
