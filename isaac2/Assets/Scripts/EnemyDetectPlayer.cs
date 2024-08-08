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
    private bool isDead;
    
    [SerializeField] private int hearingRadius;
    [SerializeField] private int sightRadius;
    [SerializeField] private float sightAngle;
    private bool canDetectOnAttack;

    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = false;  // unless some Enemies are always Alert?

        // Start detection coroutine
        StartCoroutine(AlertCheck());
    }

    // Update is called once per frame
    void Update()
    {
        isDead = this.GetComponent<Health>().GetIsDead();
    }

    /// <summary>
    ///     While Enemy is not Alert, attempt to detect Player
    ///     via hearing and vision every 0.2 seconds.
    ///     Stops when Enemy is Alerted.
    ///     NOTE: Enemy currently cannot be un-Alerted.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertCheck()
    {
        // is this the right way to do this?? I genuinely do not know
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (!isAlert && !isDead)
        {
            yield return wait;
            distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (hearingRadius >= 0)  // not deaf
            {
                EnemyHearing();
            }
            if (sightRadius >= 0 && !isAlert)  // not blind, not detected via hearing
            {
                EnemyVision();
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

    /// <summary>
    ///     Returns the max distance that an Enemy can hear the Player from.
    /// </summary>
    /// <returns>hearingRadius</returns>
    public int GetHearingRadius()
    {
        return hearingRadius;
    }

    /// <summary>
    ///     Sets the max distance that an Enemy can hear the Player from
    ///     to a new given value.
    /// </summary>
    /// <param name="newHearingRadius"></param>
    public void SetHearingRadius(int newHearingRadius)
    {
        hearingRadius = newHearingRadius;
    }

    /// <summary>
    ///     Returns the max distance that an Enemy can see the Player from.
    /// </summary>
    /// <returns>sightRadius</returns>
    public int GetSightRadius()
    {
        return sightRadius;
    }

    /// <summary>
    ///     Sets the max distance that an Enemy can see the Player from
    ///     to a new given value.
    /// </summary>
    /// <param name="newSightRadius"></param>
    public void SetSightRadius(int newSightRadius)
    {
        sightRadius = newSightRadius;
    }

    /// <summary>
    ///     Returns the angle of the Enemy's field of view.
    /// </summary>
    /// <returns></returns>
    public float GetSightAngle()
    {
        return sightAngle;
    }

    /// <summary>
    ///     Sets the angle of the Enemy's field of view
    ///     to a new given value.
    /// </summary>
    /// <param name="newSightAngle"></param>
    public void SetSightAngle(float newSightAngle)
    {
        sightAngle = newSightAngle;
    }
    


}
