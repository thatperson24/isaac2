using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursue : MonoBehaviour
{
    /// <summary>
    /// Enemies pursue Player when Alert.
    /// Enemies may keep a set following distance.
    /// 
    /// NOTES:
    /// Pursue Player
    ///     - Stay in place, never pursue
    ///     - Pursue when Alert
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
    [SerializeField] private float baseSpeed;  // constant?
    [SerializeField] private float curSpeed;
    private bool isAlert;
    [SerializeField] private float followingDistance;  // Distance from player an enemy would like to be
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAlert = this.GetComponent<EnemyDetectPlayer>().GetIsAlert(); 
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        isAlert = this.GetComponent<EnemyDetectPlayer>().GetIsAlert();
        if (isAlert && distance > followingDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, curSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    ///     Returns the current speed at which an Enemy travels.
    /// </summary>
    /// <returns>curSpeed</returns>
    public float GetCurSpeed()
    {
        return curSpeed;
    }

    /// <summary>
    ///     Set an Enemy's current speed to a given speed.
    ///     Could be used for powerups/events/etc.
    /// </summary>
    /// <param name="newSpeed"></param>
    public void SetSpeed(float newSpeed)
    {
        curSpeed = newSpeed;
    }

    /// <summary>
    ///     Increment an Enemy's current speed by a given delta amount.
    ///     Could be used for powerups/events/etc.
    /// </summary>
    /// <param name="newSpeed"></param>
    public void IncrementSpeed(float delta)
    {
        curSpeed += delta;
    }

    /// <summary>
    ///     Set an Enemy's current speed back to its base speed.
    ///     Could be used after a speed-increasing effect wears off.
    /// </summary>
    public void ResetSpeed()
    {
        curSpeed = baseSpeed;
    }

    /// <summary>
    ///     Returns the base speed at which an Enemy travels.
    /// </summary>
    /// <returns>baseSpeed</returns>
    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    /// <summary>
    ///     Returns the distance Enemy would like to be from player.
    ///     Enemy will pursue player until reaches this distance, 
    ///     and then will stop pursuit.
    ///     More for ranged Enemies - 
    ///     melee Enemies will probably have this value set to 0.
    /// </summary>
    /// <returns>followingDistance</returns>
    public float GetFollowingDistance()
    {
        return followingDistance;
    }

    /// <summary>
    ///     Sets the distance Enemy would like to be from player.
    ///     Enemy will pursue player until reaches this distance, 
    ///     and then will stop pursuit.
    ///     More for ranged combat Enemies - 
    ///     melee Enemies will probably want this value set to 0.
    /// </summary>
    /// <param name="newFollowingDistance"></param>
    public void SetFollowingDistance(float newFollowingDistance)
    {
        followingDistance = newFollowingDistance;
    }
}
