using System;
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
    ///     - TODO: Another Enemy is attacking Player
    /// Within a set sight radius & angle, Enemy can "see":
    ///     - Player
    ///     - Player Bullet
    ///     - TODO: Player flashlight cast
    ///     - TODO: Another Enemy that is Alert
    /// TODO: Enemy "touch":
    ///     - TODO: Enemy is Alert when Player touches Enemy
    ///     - TODO: Enemy is Alert when Player shoots/damages Enemy
    /// Enemy can be deaf or blind.
    /// Once Enemy is Alert / detects Player, will always stay Alert.
    /// </summary>
    
    private GameObject player;
    private const int playerLayer = 8;
    private const int enemyLayer = 9;
    private const float maxHearingRadius = 10;
    private const float maxSightRadius = 10;
    
    [SerializeField] private bool isAlert;
    private bool isDead;
    
    [Range(0,maxHearingRadius)] [SerializeField] private float hearingRadius;
    [Range(0,maxSightRadius)] [SerializeField] private float sightRadius;
    [Range(0,365)] [SerializeField] private float sightAngle;

    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = false;
        isDead = false;
        hearingRadius = Math.Clamp(hearingRadius, 0, maxHearingRadius);
        sightRadius = Math.Clamp(sightRadius, 0, maxSightRadius);
        sightAngle = Math.Clamp(sightAngle, 0, 365);

        // Start detection coroutine
        StartCoroutine(AlertCheck());
    }

    // Update is called once per frame
    void Update()
    {
        isDead = this.GetComponent<EnemyHealth>().GetIsDead();
    }

    /// <summary>
    ///     While Enemy is not Alert, attempt to detect Player
    ///     via hearing and sight every 0.2 seconds.
    ///     Stops when Enemy is Alerted.
    ///     Enemy currently cannot be un-Alerted.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertCheck()
    {
        WaitForSeconds wait = new(0.2f);

        while (!isAlert && !isDead)
        {
            yield return wait;
            if (hearingRadius >= 0)  // Not deaf
            {
                EnemyHearing();
            }
            if (sightRadius >= 0 && !isAlert)  // Not blind, not detected via hearing
            {
                EnemySight();
            }
            
        }
    }

    /// <summary>
    ///     Determines if Enemy can "hear" Player.
    ///     Currently just detects if Player within hearing radius.
    ///     Ideally would detect Player actions that make noise within that radius.
    /// </summary>
    private void EnemyHearing()
    {
        // Detects Player within radius
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        isAlert = isAlert || (distanceFromPlayer < hearingRadius);

        // TODO: detect when Player is moving (running?) in this radius - allow sneaking
        // TODO: detect when Player interacts with objects in this radius
        // TODO: detect when Player shoots Bullet in this radius
        // TODO: detect when other Enemy attacks Player in this radius
    }

    /// <summary>
    ///     Determines if Enemy can see Player (or Bullet).
    ///     Draws sector-shaped FOV with the set angle and radius,
    ///     and detects if Player is within that FOV but not obscured.
    ///     By default, facing straight upwards, but rotates with
    ///     Enemy on z-axis.
    ///     
    ///     I tried to allow for Enemy to also detect other Alert Enemy.
    ///     This doesn't work for now because Enemy was detecting itself (even though itself is not Alert).
    ///     Either would need to draw collider around Enemy or give each Enemy unique tag? I think?
    /// </summary>
    private void EnemySight()
    {
        int layerMask = (1 << playerLayer);  // Searching only in Player layer
        //int layerMask = (1 << playerLayer)|(1 << enemyLayer);  // Searching only in Player & Enemy layers

        Collider2D[] FOVCollisions = Physics2D.OverlapCircleAll(this.transform.position, sightRadius, layerMask);  // Draw circle on Player layer

        foreach (var collider in FOVCollisions)
        {
            //if ((collider.gameObject.CompareTag("Enemy") &&
            //     collider.gameObject.GetComponent<EnemyDetectPlayer>().GetIsAlert()) ||
            //    (!collider.gameObject.CompareTag("Enemy")))  // If collider belongs to Alert Enemy or any non-Enemy (Player, Bullet)
            
            Vector2 targetDirection = (collider.transform.position - this.transform.position).normalized;
            if (Vector2.Angle(this.transform.up, targetDirection) < sightAngle * 0.5)  // If target in FOV angle
            {
                float distanceFromTarget = Vector2.Distance(this.transform.position, collider.transform.position);
                
                // Alert if target not obstructed from view
                // Draw ray from Enemy to target, detect if any collisions (exclude Player layer)
                if (Physics2D.Raycast(this.transform.position, targetDirection, distanceFromTarget, ~layerMask))
                {
                    AlertEnemy();
                    return;
                }
            }
        }
        // TODO: Detect when Player's Flashlight cast within FOV?
    }



    // TODO: Detect when Player damages Enemy
    // TODO: Detect when Player touches Enemy (like from behind)


    /// <summary>
    ///     Draws FOV and hearing visualizions around Enemy in Scene Editor.
    ///     Only draws when Enemy is Idle/non-Alert.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!isAlert)  // Draw Enemy FOV when still Idle
        {
            DrawSightGizmo();
            DrawHearingGizmo();
        }
    }

    /// <summary>
    ///     Draws "solid arc" AKA circle sector in front of Enemy that visualizes their
    ///     sight radius & angle / FOV, viewable in Scene editor.
    /// </summary>
    private void DrawSightGizmo()
    {
        if (sightAngle != 0 && sightRadius >= 0)  // Don't do if blind
        {
            Vector3 start = new(
                Mathf.Sin(((sightAngle * 0.5f) - transform.eulerAngles.z) * Mathf.Deg2Rad),
                Mathf.Cos(((sightAngle * 0.5f) - transform.eulerAngles.z) * Mathf.Deg2Rad));

            Handles.color = new Color(255, 0, 0, 0.03f);
            Handles.DrawSolidArc(transform.position, Vector3.forward, start, sightAngle, sightRadius);
        }
    }

    /// <summary>
    ///     Draws circle around Enemy that visualizes their
    ///     hearing radius, viewable in Scene editor.
    /// </summary>
    private void DrawHearingGizmo()
    {
        if (hearingRadius >= 0)  // Don't do if deaf
        {
            Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, hearingRadius);
        }
    }

    // ***************************************
    // I have no previous C# experience
    // I just learned about Properties ...
    // Should I do that instead???
    // I think the answer is yes??
    // ***************************************


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
    public float GetHearingRadius()
    {
        return hearingRadius;
    }

    /// <summary>
    ///     Sets the max distance that an Enemy can hear the Player from
    ///     to a new given value.
    ///     Asserts greater than 0 and less than maxHearingRadius.
    /// </summary>
    /// <param name="newHearingRadius"></param>
    public void SetHearingRadius(int newHearingRadius)
    {
        hearingRadius = Math.Clamp(newHearingRadius, 0, maxHearingRadius);
    }

    /// <summary>
    ///     Returns the max distance that an Enemy can see the Player from.
    /// </summary>
    /// <returns>sightRadius</returns>
    public float GetSightRadius()
    {
        return sightRadius;
    }

    /// <summary>
    ///     Sets the max distance that an Enemy can see the Player from
    ///     to a new given value.
    ///     Asserts greater than 0 and less than maxSightRadius.
    /// </summary>
    /// <param name="newSightRadius"></param>
    public void SetSightRadius(int newSightRadius)
    {
        sightRadius = Math.Clamp(newSightRadius, 0, maxSightRadius);
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
    ///     Asserts within range [0, 365].
    /// </summary>
    /// <param name="newSightAngle"></param>
    public void SetSightAngle(float newSightAngle)
    {
        sightAngle = Math.Clamp(newSightAngle, 0, 365);
    }
}
