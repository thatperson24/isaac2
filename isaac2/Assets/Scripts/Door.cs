using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DOOR {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    [SerializeField] private DOOR doorLocation;

    // Start is called before the first frame update
    void Start()
    {
        SpawnTrigger();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTrigger()
    {
        GameObject playerChecker = new GameObject();
        playerChecker.AddComponent<BoxCollider2D>();
        playerChecker.GetComponent<BoxCollider2D>().isTrigger = true;
        playerChecker.name = "Player Check";

        int xOffset = 0;
        int yOffset = 0;
        switch (doorLocation) {
            case DOOR.TOP:
                yOffset = -1;
                playerChecker.GetComponent<BoxCollider2D>().size = new Vector2(2f, 1f);
                break;
            case DOOR.BOTTOM:
                yOffset = 1;
                playerChecker.GetComponent<BoxCollider2D>().size = new Vector2(2f, 1f);
                break;
            case DOOR.LEFT:
                xOffset = 1;
                playerChecker.GetComponent<BoxCollider2D>().size = new Vector2(1f, 2f);
                break;
            case DOOR.RIGHT:
                xOffset = -1;
                playerChecker.GetComponent<BoxCollider2D>().size = new Vector2(1f, 2f);
                break;
        }

        playerChecker.transform.position = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
        playerChecker.transform.parent = this.transform;
    }
}
