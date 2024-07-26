using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursue : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>

    /// TODO: enemy flee (ranged enemies especially)

    private GameObject player;
    [SerializeField] private float speed;
    private bool isAlert;
    [SerializeField] private float personalSpace;  // amount of distance from player an enemy would like to be
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
        if (isAlert && distance > personalSpace)
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
    /// Returns the distance an enemy would like to be from player.
    /// Enemy will pursue player until reaches this distance, and then
    /// will stop pursuit.
    /// More for ranged combat enemies - melee enemies
    /// will probably have this value set to 0.
    /// </summary>
    /// <returns></returns>
    public float GetPersonalSpace()
    {
        return personalSpace;
    }

    /// <summary>
    /// Sets the distance an enemy would like to be from player.
    /// Enemy will pursue player until reaches this distance, and then
    /// will stop pursuit.
    /// More for ranged combat enemies - melee enemies
    /// will probably want this value set to 0.
    /// </summary>
    /// <param name="newPersonalSpace"></param>
    public void SetPersonalSpace(float newPersonalSpace)
    {
        personalSpace = newPersonalSpace;
    }
}
