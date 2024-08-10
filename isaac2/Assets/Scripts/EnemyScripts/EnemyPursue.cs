using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursue : MonoBehaviour
{
    /// <summary>
    /// Enemies pursue Player when Alert.
    /// Enemies may keep a set follow distance 
    ///     (and eventually, flee when Player is too close).
    /// Enemies have a base speed at which they travel,
    ///     and a current speed that may be higher or lower
    ///     depending on status effects.
    /// 
    /// TODO:
    /// Pursue Player
    ///     - Elements of randomness
    ///     - Avoid obstacles & dangers
    ///     - Avoid walls & navigate corners
    /// Flee Player
    ///     - Ranged enemies flee when Player is too close.
    ///     - Low health, etc.
    /// Speed
    ///     - Base speed randomness?
    ///     - Speed changed by powerups/effects
    ///         
    /// </summary>


    private GameObject player;
    [SerializeField] private float baseSpeed;  // constant?
    [SerializeField] private float curSpeed;
    private bool isAlert;
    // Follow distance = Distance from player an Enemy would like to be
    [SerializeField] private float minFollowDistance;  // Enemy will flee when Player gets this close
    [SerializeField] private float maxFollowDistance;  // Enemy will flee/pursue until this close to Player
    // NOTE: a skittish enemy could have an infinite/large follow distance in order to always flee Player? 
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curSpeed = baseSpeed;
        isAlert = this.GetComponent<EnemyDetectPlayer>().IsAlert; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<EnemyHealth>().GetIsDead()) {  // Stop updates if Enemy is dead
            distance = Vector2.Distance(transform.position, player.transform.position);

            isAlert = this.GetComponent<EnemyDetectPlayer>().IsAlert;
            if (isAlert && distance > maxFollowDistance)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, curSpeed * Time.deltaTime);
            }
            else if (isAlert && distance < minFollowDistance)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, -curSpeed * Time.deltaTime);
            }
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
    public void SetCurSpeed(float newSpeed)
    {
        curSpeed = newSpeed;
    }

    /// <summary>
    ///     Increment an Enemy's current speed by a given delta amount.
    ///     Could be used for powerups/events/etc.
    /// </summary>
    /// <param name="newSpeed"></param>
    public void IncrementCurSpeed(float delta)
    {
        curSpeed += delta;
    }

    /// <summary>
    ///     Set an Enemy's current speed back to its base speed.
    ///     Could be used after a speed-increasing effect wears off.
    /// </summary>
    public void ResetCurSpeed()
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
    ///     Returns the minimum distance Enemy would like to be from Player.
    ///     If Player approaches Enemy within this distance, Enemy will flee
    ///     to maxFollowingDistance.
    ///     More for ranged Enemies - 
    ///     Melee Enemies will probably have this value set to 0.
    /// </summary>
    /// <returns>minFollowDistance</returns>
    public float GetMinFollowDistance()
    {
        return minFollowDistance;
    }

    /// <summary>
    ///     Returns the maxmium distance Enemy would like to be from Player.
    ///     Enemy will pursue Player until reaches this distance, 
    ///     and then will stop pursuit.
    ///     If Player gets too close (minFollowingDistance), Enemy will flee
    ///     to this distance.
    ///     More for ranged Enemies - 
    ///     Melee Enemies will probably have this value set to 0.
    /// </summary>
    /// <returns>maxFollowDistance</returns>
    public float GetMaxFollowDistance()
    {
        return maxFollowDistance;
    }

    /// <summary>
    ///     Sets the minimum distance Enemy would like to be from Player to given value.
    ///     If Player approaches Enemy within this distance, Enemy will flee
    ///     to maxFollowingDistance.
    ///     More for ranged Enemies - 
    ///     Melee Enemies will probably have this value set to 0.
    /// </summary>
    /// <param name="newMinFollowDistance"></param>
    public void SetMinFollowDistance(float newMinFollowDistance)
    {
        minFollowDistance = newMinFollowDistance;
    }

    /// <summary>
    ///     Sets the maxmium distance Enemy would like to be from Player to given value.
    ///     Enemy will pursue Player until reaches this distance, 
    ///     and then will stop pursuit.
    ///     If Player gets too close (minFollowingDistance), Enemy will flee
    ///     to this distance.
    ///     More for ranged Enemies - 
    ///     Melee Enemies will probably have this value set to 0.
    /// </summary>
    /// <param name="newMaxFollowDistance"></param>
    public void SetMaxFollowDistance(float newMaxFollowDistance)
    {
        maxFollowDistance = newMaxFollowDistance;
    }
}
