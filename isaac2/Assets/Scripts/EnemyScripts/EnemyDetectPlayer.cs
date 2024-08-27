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
    /// Enemy detects if Player is nearby based on Hearing, Sight, and Touch.
    /// Within a set hearing radius, Enemy can "hear":
    ///     + Player simply existing
    ///     - TODO: Player shooting a Bullet (Need to figure out how to do this)
    ///     - TODO: Player moving fast/loudly? (Need Player movement variation)
    ///     - TODO: Player interacting with an object (Need Items/Player interactions)
    ///     - TODO: Another Enemy is attacking Player (Need Enemy attacks)
    /// Within a set sight radius & angle, Enemy can "see":
    ///     + Player
    ///     + Player Bullet
    ///     - TODO: Player flashlight cast (Need to figure out how to do this)
    ///     - TODO: Another Enemy that is Alert (Need to figure out how to do this)
    /// Enemy can sense Player using "touch" when:
    ///     + Player touches Enemy
    ///     + Player shoots Enemy (Bullet class initiates)
    /// Enemy can be deaf or blind.
    /// Once Enemy is Alert / detects Player, will always stay Alert.
    /// </summary>
    private const int PlayerLayer = 8;
    // private const int EnemyLayer = 9;
    private const float MaxHearingRadius = 10;
    private const float MaxSightRadius = 10;
    
    private GameObject player;  // static?

    // Properties
    [field: SerializeField] public bool IsAlert { get; private set; }
    // Max distance that an Enemy can "hear" the Player from.
    [Range(0, MaxHearingRadius)] [SerializeField] private float _hearingRadius;
    public float HearingRadius
    {
        get => this._hearingRadius;
        private set => this._hearingRadius = Math.Clamp(value, 0, MaxHearingRadius);
    }
    // Max distance that an Enemy can "see" the Player from
    [Range(0, MaxSightRadius)] [SerializeField] private float _sightRadius;
    public float SightRadius
    {
        get => this._sightRadius;
        private set => this._sightRadius = Math.Clamp(value, 0, MaxSightRadius);
    }
    // Angle of Enemy FOV
    [Range(0, 360)] [SerializeField] private float _sightAngle;
    public float SightAngle
    {
        get => this._sightAngle;
        private set => this._sightAngle = Math.Clamp(value, 0, 360);
    }
    // Whether or not Enemy can detect Player location when shot/attacked
    [field: SerializeField] public bool CanDetectOnAttack{ get; private set; }




    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        this.IsAlert = false;
        this.HearingRadius = Math.Clamp(this.HearingRadius, 0, MaxHearingRadius);
        this.SightRadius = Math.Clamp(this.SightRadius, 0, MaxSightRadius);
        this.SightAngle = Math.Clamp(this.SightAngle, 0, 365);

        // Start detection coroutine
        StartCoroutine(AlertCheck());
    }

    // Update is called once per frame
    // void Update()
    // {
    // }

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

        while (!this.IsAlert && !this.GetComponent<EnemyHealth>().IsDead())
        {
            yield return wait;
            if (!this.IsDeaf())
            {
                EnemyHearing();
            }
            if (!this.IsBlind() && !this.IsAlert)
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
        this.IsAlert = this.IsAlert || (distanceFromPlayer < this.HearingRadius);

        // TODO: detect when Player is moving (running?) in this radius - allow sneaking
        // TODO: detect when Player interacts with objects in this radius
        // TODO: detect when Player shoots Bullet in this radius
        // TODO: detect when other Enemy attacks Player in this radius

        // Need to figure out how I want to use Player actions to Alert Enemies that have different levels of hearing
        // I set a max hearing radius so I could maybe draw a circle around Player with max hearing radius
        // Then check each Enemy in range's hearing radius to see if they can hear it??
        // Worried that this is too much though
        // I could also set fields on Player that signal for example that a shot has been fired recently
        // Then Enemy checks for Player in radius that has that value set to true?
        // Idk............
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
    ///     
    ///     I also tried to add a collider to the flashlight but failed...
    /// </summary>
    private void EnemySight()
    {
        int layerMask = 1 << PlayerLayer;  // Searching only in Player layer
        //int layerMask = (1 << playerLayer)|(1 << enemyLayer);  // Searching only in Player & Enemy layers

        Collider2D[] FOVCollisions = Physics2D.OverlapCircleAll(this.transform.position, this.SightRadius, layerMask);  // Draw circle on Player layer

        foreach (var collider in FOVCollisions)
        {
            //if ((collider.gameObject.CompareTag("Enemy") &&
            //     collider.gameObject.GetComponent<EnemyDetectPlayer>().GetIsAlert()) ||
            //    (!collider.gameObject.CompareTag("Enemy")))  // If collider belongs to Alert Enemy or any non-Enemy (Player, Bullet)
            
            Vector2 targetDirection = (collider.transform.position - this.transform.position).normalized;
            if (Vector2.Angle(this.transform.up, targetDirection) < this.SightAngle * 0.5)  // If target in FOV angle
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
        // I tried to do this by simply adding a custom collider to the Spot Light 2D game object
        // and ensuring it was on the Player layer but nothing happened
        // Need to think about this some more
    }

    /// <summary>
    ///     Detects any collision with Enemy, and
    ///     Alerts Enemy if Player touches Enemy.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.IsAlert && collision.gameObject.CompareTag("Player"))
        {
            AlertEnemy();
        }
    }

    /// <summary>
    ///     Draws FOV and hearing visualizions around Enemy in Scene Editor.
    ///     Only draws when Enemy is Idle/non-Alert.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!this.IsAlert)  // Draw Enemy FOV when still Idle
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
        if (!this.IsBlind())
        {
            Vector3 start = new(
                Mathf.Sin(((this.SightAngle * 0.5f) - this.transform.eulerAngles.z) * Mathf.Deg2Rad),
                Mathf.Cos(((this.SightAngle * 0.5f) - this.transform.eulerAngles.z) * Mathf.Deg2Rad));

            Handles.color = new Color(255, 0, 0, 0.03f);
            Handles.DrawSolidArc(this.transform.position, Vector3.forward, start, this.SightAngle, this.SightRadius);
        }
    }

    /// <summary>
    ///     Draws circle around Enemy that visualizes their
    ///     hearing radius, viewable in Scene editor.
    /// </summary>
    private void DrawHearingGizmo()
    {
        if (!this.IsDeaf())
        {
            Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, Vector3.forward, this.HearingRadius);
        }
    }

    /// <summary>
    ///     Set "isAlert" bool to true,
    ///     i.e., make Enemy aware of Player location.
    /// </summary>
    public void AlertEnemy()
    {
        this.IsAlert = true;
    }

    /// <summary>
    ///     Returns whether or not an Enemy is blind,
    ///     i.e., sight radius and/or angle is 0.
    ///     Can this also be a Property?
    /// </summary>
    /// <returns></returns>
    public bool IsBlind()
    {
        return (this.SightRadius <= 0) || (this.SightAngle <= 0);
    }

    /// <summary>
    ///     Returns whether or not an Enemy is deaf,
    ///     i.e., hearing radius is 0.
    ///     Can this also be a Property?
    /// </summary>
    /// <returns></returns>
    public bool IsDeaf()
    {
        return this.HearingRadius <= 0;
    }

    /// <summary>
    ///     When Bullet hits Enemy, calls this method,
    ///     which Alerts Enemy if possible.
    ///     Enemy can be set to not CanDetectOnAttack.
    /// </summary>
    public void EnemyAttacked()
    {
        if (!this.IsAlert && this.CanDetectOnAttack)
        {
            AlertEnemy();
        }
    }
}
