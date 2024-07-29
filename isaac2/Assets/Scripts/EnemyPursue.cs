using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursue : MonoBehaviour
{
    /// <summary>
    /// Enemies pursue Player when alert.
    /// Enemies may keep a set following distance.
    /// 
    /// NOTES:
    /// Pursue Player
    ///     - Stay in place, never pursue
    ///     - Pursue when alert
    ///     - Following distance
    ///         - Always try to touch Player (melee & explosive attacks, etc.)
    ///         - Keep a set distance away (ranged attacks, etc.)
    ///             - Flee when Player gets closer than following distance
    ///             - Allow Player to get close
    ///     - Pathing
    ///         - Elements of randomness
    ///         - Avoid obstacles & dangers
    ///         - Avoid walls & navigate corners
    /// Flee Player
    ///     - Enemies could flee under certain conditions (low health, ranged attacks)
    /// Speed
    ///     - Not the current velocity, but how "fast" an enemy is?
    ///     - Base speed
    ///         - Set on spawn
    ///         - Different types of enemy, different speeds
    ///         - Randomly selected from range
    ///     - Speed
    ///         - Can be changed by powerups/effects
    ///         
    /// </summary>


    private GameObject player;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speed;
    private bool isAlert;
    [SerializeField] private float followingDistance;  // amount of distance from player an enemy would like to be
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = this.GetComponent<EnemyDetectPlayer>().getIsAlert(); 
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        isAlert = this.GetComponent<EnemyDetectPlayer>().getIsAlert();
        if (isAlert && distance > followingDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Returns the speed at which an Enemy travels.
    /// </summary>
    /// <returns></returns>
    public float GetSpeed()
    {
        return speed;
    }

    /// <summary>
    /// Set an Enemy's speed to a given speed.
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    /// <summary>
    /// Increment an Enemy's speed by a given delta amount.
    /// </summary>
    /// <param name="newSpeed"></param>
    public void IncrementSpeed(float delta)
    {
        speed += delta;
    }

    /// <summary>
    /// Set an Enemy's speed back to its base speed.
    /// </summary>
    public void ResetSpeed()
    {
        speed = baseSpeed;
    }

    /// <summary>
    /// Returns the base speed at which an Enemy travels.
    /// </summary>
    /// <returns></returns>
    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    /// <summary>
    /// Returns the distance an enemy would like to be from player.
    /// Enemy will pursue player until reaches this distance, and then
    /// will stop pursuit.
    /// More for ranged combat enemies - melee enemies
    /// will probably have this value set to 0.
    /// </summary>
    /// <returns></returns>
    public float GetFollowingDistance()
    {
        return followingDistance;
    }

    /// <summary>
    /// Sets the distance an enemy would like to be from player.
    /// Enemy will pursue player until reaches this distance, and then
    /// will stop pursuit.
    /// More for ranged combat enemies - melee enemies
    /// will probably want this value set to 0.
    /// </summary>
    /// <param name="newFollowingDistance"></param>
    public void SetFollowingDistance(float newFollowingDistance)
    {
        followingDistance = newFollowingDistance;
    }
}
