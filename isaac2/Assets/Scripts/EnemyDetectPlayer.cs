using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    /// <summary>
    /// Enemies detect if Player is nearby based on proximity.
    /// Enemies may not detect Player until rather close, but will take
    /// longer to forget.
    /// - Would like to implement detection based on gunfire as well. 
    ///     (i.e., even if Player is far away, Enemy will detect Player if Player is shooting at Enemy)
    /// - Could also take into account time for forgetfulness.
    /// - Could drop this entirely and make enemies alert at all times.
    /// - Detection could be based on direction enemy is facing / field of view.
    /// - Could detect when in the same room.
    /// </summary>
    [SerializeField] private float detectDistance;
    [SerializeField] private float forgetDistance;
    private GameObject player;
    private float distance;
    [SerializeField] private bool isAlert;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        
        // condense this
        if (isAlert) 
        {
            if (distance > forgetDistance)
            {
                isAlert = false;
            }
        }
        else 
        {
            if (distance < detectDistance)
            {
                isAlert = true;
            }
        }
    }

    /// <summary>
    /// Return whether or not the current enemy is alert 
    /// (i.e., aware of player and actively pursuing).
    /// </summary>
    /// <returns></returns>
    public bool getIsAlert()
    {
        return isAlert;
    }

    // TODO: collision detection? if bullet collides with enemy, 
    // enemy is alert for x amount of time?
}
