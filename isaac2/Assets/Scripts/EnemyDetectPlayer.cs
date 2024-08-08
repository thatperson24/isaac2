using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyDetectPlayer : MonoBehaviour
{
    /// <summary>
    /// Enemy detects if Player is nearby based on Hearing and Sight.
    /// Within a set hearing radius, Enemy can "hear":
    ///     - Player simply existing
    ///     - TODO: Player moving fast/loudly?
    ///     - TODO: Player interacting with an object
    ///     - TODO: Player shooting a Bullet
    /// Within a set sight radius & angle, Enemy can "see":
    ///     - Player
    ///     - Player Bullet
    ///     - TODO: Player flashlight cast
    /// Enemy can be deaf or blind.
    /// Once Enemy is Alert / detects Player, will always stay Alert.
    /// </summary>
    
    private GameObject player;
    
    [SerializeField] private bool isAlert;
    private bool isDead;
    
    [SerializeField] private int hearingRadius;
    [SerializeField] private int sightRadius;
    [SerializeField] private float sightAngle;

    
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
    ///     via hearing and sight every 0.2 seconds.
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
            if (hearingRadius >= 0)  // not deaf
            {
                EnemyHearing();
            }
            if (sightRadius >= 0 && !isAlert)  // not blind, not detected via hearing
            {
                EnemySight();
            }
            
        }
    }

    /// <summary>
    ///     Determines if Enemy can see Player (or Bullet).
    ///     Draws sector-shaped FOV with the set angle and radius,
    ///     and detects if Player is within that FOV but not obscured.
    /// </summary>
    private void EnemySight()
    {
        int playerLayer = 8;
        int layerMask = 1 << playerLayer;
        Collider2D[] FOV = Physics2D.OverlapCircleAll(this.transform.position, sightRadius, layerMask);  // draw radius on Player layer (8)

        if(FOV.Length > 0)  // if any entity (Player or Bullet) detected in radius
        {
            Collider2D playerCollider = FOV[0];
            Vector2 directionToPlayer = (playerCollider.transform.position - this.transform.position).normalized;

            if (Vector2.Angle(this.transform.right, directionToPlayer) < sightAngle * 0.5)  // if Player/Bullet in FOV angle
            {
                float distanceToPlayer = Vector2.Distance(this.transform.position, playerCollider.transform.position);
                
                // isAlert = true if Player/Bullet not obstructed from view
                // Draw ray from Enemy to Player/Bullet, detect if any collisions (exclude Player layer 8)
                if (Physics2D.Raycast(this.transform.position, directionToPlayer, distanceToPlayer, ~layerMask))
                {
                    isAlert = true;
                }
            }
        }
        // TODO: Detect when Player's Flashlight cast within FOV?
    }

    /// <summary>
    ///     Determines if Enemy can "hear" Player.
    ///     Currently just detects if Player within hearing radius.
    ///     Ideally would detect Player actions that make noise within that radius.
    /// </summary>
    private void EnemyHearing()
    {
        // Detects Player within small radius
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        isAlert = isAlert || (distanceFromPlayer < hearingRadius);
        // TODO: detect when Player is moving (running?) in this radius - allow sneaking
        // TODO: detect when Player interacts with objects in this radius
        // TODO: detect when Player shoots Bullet in this radius
    }

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
