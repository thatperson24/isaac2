using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Connected rooms
    [SerializeField] private GameObject topRoom;
    [SerializeField] private GameObject bottomRoom;
    [SerializeField] private GameObject leftRoom;
    [SerializeField] private GameObject rightRoom;

    //Checks for if the door of this room is connected to the doors of another room
    [SerializeField] private Transform topDoorCheck;
    [SerializeField] private Transform bottomDoorCheck;
    [SerializeField] private Transform leftDoorCheck;
    [SerializeField] private Transform rightDoorCheck;

    [Header("Wall Checks")] //Checks for if the walls of this room are touching walls of another room
    [SerializeField] private Transform topWallCheck;
    [SerializeField] private Transform bottomWallCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;

    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private LayerMask wallLayer;

    //On initalization
    void Awake()
    {
        this.gameObject.name = this.gameObject.name + ": " + this.gameObject.transform.position.x + " - " + this.gameObject.transform.position.y;
        
        if (WallCheck())
        {
            //Check if the room has a top door and if that top door is connected to another door.
            if (topDoorCheck != null && Physics2D.OverlapCircle(topDoorCheck.position, .25f, doorLayer) != null)
            {
                //Attaching the rooms to each other
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
    }

    /*The function sets an adjacent room as the new room
     Used by newly instantiated rooms*/
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

    public int GetNumOpenings()
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

    /*The function verifies that doors are touching doors and walls are touching walls
     RETURNS:
        - TRUE if all walls and doors are valid.
        - FALSE if some walls and doors are mismatched*/
    public bool WallCheck()
    {
        //Verify if a wall check exists and if it is touching a door
        if ((topWallCheck != null && Physics2D.OverlapCircle(topWallCheck.position, .25f, doorLayer) != null) ||
            (bottomWallCheck != null && Physics2D.OverlapCircle(bottomWallCheck.position, .25f, doorLayer) != null) ||
            (leftWallCheck != null && Physics2D.OverlapCircle(leftWallCheck.position, .25f, doorLayer) != null) ||
            (rightWallCheck != null && Physics2D.OverlapCircle(rightWallCheck.position, .25f, doorLayer) != null))
        {            
            Destroy(this.gameObject);
            return false;
        }
        //Verify if a door check exists and if its touching a wall
        if ((topDoorCheck != null && Physics2D.OverlapCircle(topDoorCheck.position, .25f, wallLayer) != null) ||
            (bottomDoorCheck != null && Physics2D.OverlapCircle(bottomDoorCheck.position, .25f, wallLayer) != null) ||
            (leftDoorCheck != null && Physics2D.OverlapCircle(leftDoorCheck.position, .25f, wallLayer) != null) ||
            (rightDoorCheck != null && Physics2D.OverlapCircle(rightDoorCheck.position, .25f, wallLayer) != null))
        {
            Destroy(this.gameObject);
            return false;
        }
        else
        {
            return true;
        }
        
    }

    /*The function deactivates the door game object if it is not connected to a room*/
    public void CloseRooms()
    {
        if (topRoom == null)
        {
            this.gameObject.transform.Find("Doors").Find("Top").gameObject.SetActive(false);
        }
        if (bottomRoom == null)
        {
            this.gameObject.transform.Find("Doors").Find("Bottom").gameObject.SetActive(false);
        }
        if (leftRoom == null)
        {
            this.gameObject.transform.Find("Doors").Find("Left").gameObject.SetActive(false);
        }
        if (rightRoom == null) 
        {
            this.gameObject.transform.Find("Doors").Find("Right").gameObject.SetActive(false);
        }
    }
}
