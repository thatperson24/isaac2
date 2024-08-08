using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

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
    
    private float distanceFromPlayer;
    private GameObject player;
    
    [SerializeField] private bool isAlert;
    
    [SerializeField] private float hearingRadius;
    [SerializeField] private float sightRadius;
    [SerializeField] private float sightAngle;
    private bool canDetectOnAttack;

    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = false;  // unless some enemies are always Alert?

        // Start detection coroutine
        StartCoroutine(AlertCheck());
    }

    // Update is called once per frame
    // void Update()
    // {
    //      distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
    //      isAlert = isAlert || (distanceFromPlayer < hearingRadius);
    // }

    private IEnumerator AlertCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (!isAlert)
        {
            yield return wait;
            distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (sightRadius >= 0)  // not blind
            {
                EnemyVision();
            }
            if (hearingRadius >= 0 && !isAlert)  // not deaf, not detected via vision
            {
                EnemyHearing();
            }
        }
    }

    private void EnemyVision()
    {
        // TODO: Create FOV cone
        // TODO: Detect when Player within FOV
        // TODO: Detect when Player's Flashlight within FOV
        // TODO: Detect when Player's Bullet within FOV
    }

    private void EnemyHearing()
    {
        // Detects Player within small radius
        isAlert = isAlert || (distanceFromPlayer < hearingRadius);
        // TODO: detect when Player is moving (running?) in this radius - allow sneaking
        // TODO: detect when Player interacts with objects in this radius
        // TODO: detect when Player shoots Bullet in this radius
    }

    // TODO: Collision detection, if bullet collides with Enemy, set to Alert
    //      - "dumb" Enemy may not be Alert on attack if can't see Player


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
