using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    /// <summary>
    /// Enemies detect if Player is nearby based on radial proximity.
    /// 
    /// TODO:
    /// Detect player/begin attacking...
    ///     - When Player enters Enemy field of vision
    ///     - When Player attacks
    ///         - Enemy can't see Player, Player attacks, enemy is Alert
    ///         - Enemy can't see Player, Player attacks, enemy is confused & ignores
    ///     - When Player shines light at Enemy
    ///     - When Player interacts with an object/makes noise
    ///     - Enemy never forgets after detecting Player
    /// </summary>
    
    [SerializeField] private bool isAlert;
    private GameObject player;

    [SerializeField] private float detectDistance;  // TODO: Refactor into realistic FOV
    private float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = false;  // unless some enemies are always Alert?
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        
        isAlert = isAlert || (distance < detectDistance);
        // TODO: Implement realistic angular line of sight
        // TODO: Factor in obstacles blocking line of sight
        // TODO: Consider blind enemies?
    }


    // TODO: Collision detection, if bullet collides with Enemy, set to Alert
    //      - "dumb" Enemy may not be Alert on attack if can't see Player
    // TODO: Light detection, if flashlight shone at Enemy, set to Alert
    // TODO: "sound" detection, if Player interacts with objects, set Enemies in radius to Alert
    //      - Would a 360 hearing radius make sense as well, such as for blind enemies?
    //      - Consider deaf enemies?


    /// <summary>
    ///     Return whether or not the current enemy is Alert
    ///     (i.e., aware of player and actively pursuing).
    /// </summary>
    /// <returns>isAlert</returns>
    public bool GetIsAlert()
    {
        return isAlert;
    }

    /// <summary>
    ///     Set "isAlert" bool to given new value.
    ///     Private for now but depending on how we want to
    ///     use this, could be made public.
    /// </summary>
    /// <param name="newIsAlert"></param>
    private void SetIsAlert(bool newIsAlert)
    {
        isAlert = newIsAlert;
    }

    /// <summary>
    ///     Set "isAlert" bool to true,
    ///     i.e., make Enemy aware of Player location.
    /// </summary>
    public void AlertEnemy()
    {
        SetIsAlert(true);
    }
}
