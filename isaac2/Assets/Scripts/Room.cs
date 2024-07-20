using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject topRoom;
    [SerializeField] private GameObject bottomRoom;
    [SerializeField] private GameObject leftRoom;
    [SerializeField] private GameObject rightRoom;

    [SerializeField] private Transform topDoorCheck;
    [SerializeField] private Transform bottomDoorCheck;
    [SerializeField] private Transform leftDoorCheck;
    [SerializeField] private Transform rightDoorCheck;

    [SerializeField] private LayerMask doorLayer;
    // Start is called before the first frame update
    void Awake()
    {
        if (topDoorCheck != null && Physics2D.OverlapCircle(topDoorCheck.position, .25f, doorLayer) != null)
        {
            topRoom = Physics2D.OverlapCircle(topDoorCheck.position, .25f, doorLayer).gameObject.transform.parent.gameObject.transform.parent.gameObject; ;
            topRoom.GetComponent<Room>().SetRoom("Bottom", this.gameObject);
        }
        if (bottomDoorCheck != null && Physics2D.OverlapCircle(bottomDoorCheck.position, .25f, doorLayer) != null)
        {
            bottomRoom = Physics2D.OverlapCircle(bottomDoorCheck.position, .25f, doorLayer).gameObject.transform.parent.gameObject.transform.parent.gameObject;
            bottomRoom.GetComponent<Room>().SetRoom("Top", this.gameObject);
        }
        if (leftDoorCheck != null && Physics2D.OverlapCircle(leftDoorCheck.position, .25f, doorLayer) != null)
        {
            leftRoom = Physics2D.OverlapCircle(leftDoorCheck.position, .25f, doorLayer).gameObject.transform.parent.gameObject.transform.parent.gameObject;
            leftRoom.GetComponent<Room>().SetRoom("Right", this.gameObject);
        }
        if (rightDoorCheck != null && Physics2D.OverlapCircle(rightDoorCheck.position, .25f, doorLayer) != null)
        {
            rightRoom = Physics2D.OverlapCircle(rightDoorCheck.position, .25f, doorLayer).gameObject.transform.parent.gameObject.transform.parent.gameObject;
            rightRoom.GetComponent<Room>().SetRoom("Left", this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoom(string direction, GameObject room)
    {
        if (direction.Equals("Top"))
        {
            topRoom = room;
        } 
        else if (direction.Equals("Bottom"))
        {
            bottomRoom = room;
        }
        else if (direction.Equals("Left"))
        {
            leftRoom = room;
        }
        else if (direction.Equals("Right"))
        {
            rightRoom = room;
        }
    }

    public GameObject GetTopRoom()
    {
        return topRoom;
    }

    public GameObject GetBottomRoom()
    {
        return bottomRoom;
    }

    public GameObject GetLeftRoom()
    {
        return leftRoom;
    }

    public GameObject GetRightRoom()
    {
        return rightRoom;
    }

    public int GetEmptyNumOpenings()
    {
        int numOpenings = 0;
        if (topRoom == null)
        {
            numOpenings++;
        }
        if (bottomRoom == null)
        {
            numOpenings++;
        }
        if (leftRoom == null)
        {
            numOpenings++;
        }
        if (rightRoom == null)
        {
            numOpenings++;
        }
        return numOpenings;
    }
}
