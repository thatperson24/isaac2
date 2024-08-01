using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    /// <summary>
    /// IDK C# DOCUMENTATION CONVENTIONS HELP!!!!!!!!!
    /// 
    /// Health class stores entity health and deals with setting, incrementing,
    /// and fetching both current and max health.
    /// Also detects death and overheal.
    /// Used by other scripts.
    /// </summary>

    // TODO: if we want health bars they could be handled here

    [SerializeField] private int maxHealth;
    [SerializeField] private int curHealth;
    [SerializeField] private bool isDead;
    
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    //void Update()
    //{
        // Could implement gradual natural healing over time I guess?
        // otherwise can we delete this method entirely (Unity question)
    //}

    /// <summary>
    ///     Returns current health of entity.
    /// </summary>
    /// <returns>curHealth</returns>
    public int GetCurHealth()
    {
        return curHealth;
    }

    /// <summary>
    ///     Returns current maxHealth of entity.
    /// </summary>
    /// <returns>maxHealth</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    ///     Set current health to a new health value.
    ///     Prevents health modification to already dead entities.
    ///     Detects and prevents overheal.
    ///     Detects death and prevents negative health.
    ///     Resists attemps to heal/damage a dead entity.
    ///     Returns new curHealth in case caller needs (detect death etc.).
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>curHealth</returns>
    private int SetCurHealth(int newHealth) 
    {
        if (isDead || curHealth == 0) // ALREADY DEAD
        {
            return curHealth;  // prevent healing of dead entities
        }
        else if (newHealth > maxHealth) // OVERHEAL
        {
            curHealth = maxHealth;
            // TODO: overheal - could block use of heal item or convert heal to something else
        }
        else if (newHealth <= 0) // YOU DIED
        {
            curHealth = 0;
            EnemyDeath();
            
            // obviously death differs between players and enemies so might need to
            // have separate health scripts for each to account for that?
            // enemies - either despawn or just chill as a corpse
            // player - uhhhhhhhhhhhhhhhh
        }
        else
        {
            curHealth = newHealth;
        }
        return curHealth;
    }

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
        return SetCurHealth(curHealth + delta);
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
        return SetCurHealth(maxHealth);
    }

    /// <summary>
    ///     Set max health to given amount.
    ///     Could be used for powerups etc.
    ///     Resists attempts to set maxHealth to 0 or less.
    ///     Returns maxHealth in case caller needs.
    /// </summary>
    /// <param name="newMaxHealth"></param>
    /// <returns>maxHealth</returns>
    public int SetMaxHealth(int newMaxHealth) 
    {
        if (newMaxHealth <= 0)
        {
            // Resist change (or set to 1?)
            return maxHealth;
        }
        // TODO: Maybe put upper bounds on maxHealth
        maxHealth = newMaxHealth;
        return maxHealth;
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
        // TODO: deal with constraints / error checking
        return SetMaxHealth(maxHealth + delta);
    }

    /// <summary>
    ///     Return whether or not entity is dead.
    /// </summary>
    /// <returns>isDead</returns>
    public bool GetIsDead()
    {
        return isDead;
    }

    /// <summary>
    ///     Initiate Enemy death process.
    /// </summary>
    public void EnemyDeath()
    {
        isDead = true;
        this.GetComponent<EnemyPursue>().SetCurSpeed(0);
        // TODO: unalert/stop detecting player?
        // TODO: remove entity
        // TODO: replace entity with "corpse" entity with no AI (or collision?)
    }
}
