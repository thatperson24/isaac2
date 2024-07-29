using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    /// <summary>
    /// Enemies detect if Player is nearby based on proximity.
    /// Enemies may not detect Player until rather close, but will take
    /// longer to forget.
    /// 
    /// NOTES:
    /// Detect player/begin attacking
    ///     - When Player is x distance away
    ///     - When Player attacks
    ///         - Enemy is passive only until attacked
    ///         - Enemy can't see Player, Player attacks, enemy is alert
    ///         - Enemy can't see Player, Player attacks, enemy is confused & ignores
    ///     - When Player enters Enemy field of vision
    ///     - When Player shines light at Enemy
    ///     - When Player interacts with an object/makes noise
    ///     - When Player enters room
    ///     - Enemy is always alert
    /// Forget player/stop attacking
    ///     - When Player gets x distance away
    ///     - When x amount of time passes
    ///     - When Player leaves Enemy FOV
    ///     - When Player takes specific action to hide
    ///     - When Player leaves room
    ///     - Enemy never forgets after detecting Player
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
