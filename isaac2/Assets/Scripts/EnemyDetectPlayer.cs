using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float detectDistance;
    [SerializeField] private float forgetDistance;
    private float distance;
    public bool isAlert;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        
        // condense this
        if (isAlert) 
        {
            if (distance > forgetDistance)
            {
                isAlert = false;
            }
        }
        else 
        {
            if (distance < detectDistance)
            {
                isAlert = true;
            }
        }
    }
}
