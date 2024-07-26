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

    [SerializeField] private int maxHealth;  // or float?
    private int health;
    
    // Start is called before the first frame update
    void Start()
    {
        // Does health need to be initialized here ever? (Unity question)
        // If not needed can we delete this method or what
    }

    // Update is called once per frame
    void Update()
    {
        // Could implement gradual natural healing over time I guess?
    }

    /// <summary>
    /// Returns current health of entity.
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Returns current maxHealth of entity.
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Set current health to a new health value.
    /// Prevents health modification to already dead entities.
    /// Detects and prevents overheal.
    /// Detects death and prevents negative health.
    /// Returns new health in case caller needs (detect death etc.).
    /// </summary>
    /// <param name="amount"></param>
    public int SetHealth(int newHealth) 
    {
        if (health == 0) // ALREADY DEAD
        {
            return health;  // prevent healing of dead entities
        }
        else if (newHealth > maxHealth) // OVERHEAL
        {
            health = maxHealth;
            // overheal - could block use of heal item or convert heal to something else
        }
        else if (newHealth <= 0) // YOU DIED
        {
            health = 0;
            // initiate death stuff - 
            // obviously differs between players and enemies so might need to
            // have separate health scripts for each to account for that?
            // enemies - either despawn or just chill as a corpse
            // player - uhhhhhhhhhhhhhhhh
        }
        else
        {
            health = newHealth;
        }
        return health;
    }

    /// <summary>
    /// Increment current health by given + or - delta amount.
    /// Calls setHealth which handles death, overheal, etc.
    /// Delta is 1 by default (enemy heals by 1 HP).
    /// Returns new health in case caller needs (detect death, etc.).
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public int IncrementHealth(int delta = 1)
    {
        return SetHealth(health + delta);
    }

    /// <summary>
    /// Decrement current health (damage) by given delta amount.
    /// Delta is 1 by default (entity takes 1 HP damage).
    /// Returns new health in case caller needs (detect death, etc.).
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public int Damage(int delta = 1)
    {
        return IncrementHealth(-delta); 
        // convention question: is damage param negative or positive?
        // TODO: visual cues?
    }

    /// <summary>
    /// Increment current health (heal) by given delta amount.
    /// Delta is 1 by default (entity heals 1 HP).
    /// Returns new health in case caller needs.
    /// Might want to return overheal flag???
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public int Heal(int delta = 1)
    {
        return IncrementHealth(delta);
        // TODO: visual cues?
    }

    /// <summary>
    /// Set current health to the max health / fully heal entity.
    /// Returns current maxHealth (probably unnecessary).
    /// </summary>
    /// <returns></returns>
    public int FullHeal() // or resetHealth() ??
    {
        return SetHealth(maxHealth);
        // TODO: visual cues?
    }

    /// <summary>
    /// Set max health to given amount.
    /// Could be used for powerups etc.
    /// Returns maxHealth in case caller needs.
    /// </summary>
    /// <param name="newMaxHealth"></param>
    /// <returns></returns>
    public int SetMaxHealth(int newMaxHealth) 
    {
        if (newMaxHealth <= 0)  // THATS ILLEGAL
        {
            // ERROR!! resist change (or maybe set to 1?)
            // TODO: throw error?
            return maxHealth;
        }
        // TODO: maybe put max constraints on maxHealth

        maxHealth = newMaxHealth;
        return maxHealth;
    }

    /// <summary>
    /// Increment maxHealth by given delta amount.
    /// Delta is 1 by default (entity gets 1 more max HP).
    /// Could be used for powerups etc.
    /// Returns new maxHealth in case caller needs.
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public int IncrementMaxHealth(int delta = 1) 
    {
        // TODO: deal with constraints / error checking
        return SetMaxHealth(maxHealth + delta);
    }
}
