using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    /// <summary>
    /// Just stores health. 
    /// </summary>
    [SerializeField] private int maxHealth;  // or float?
    private int health;
    
    // Start is called before the first frame update
    void Start()
    {
        // Does health need to be initialized here ever? (Unity question)
    }

    // Update is called once per frame
    void Update()
    {
        // Could implement gradual healing over time I guess?
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int getHealth()
    {
        return health;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void modifyHealth(int amount = 1)
    {
        health += amount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void damage(int amount = 1)
    {
        modifyHealth(-amount); 
        // convention question - damage param negative or positive?
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void heal(int amount = 1)
    {
        modifyHealth(amount);
    }
}
