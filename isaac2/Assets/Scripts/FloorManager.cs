using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private int xScale; //Scales according to the x/y scales of the room game objects
    [SerializeField] private int yScale;

    [SerializeField] private int maxNumRooms;
    [SerializeField] private int currentNumRooms = 1;
    [SerializeField] private int currentNumOpenings;

    [SerializeField] private List<GameObject> startingRooms;
    [SerializeField] private List<GameObject> topConPieces;
    [SerializeField] private List<GameObject> bottomConPieces;
    [SerializeField] private List<GameObject> leftConPieces;
    [SerializeField] private List<GameObject> rightConPieces;

    [SerializeField] private GameObject closingPiece;

    [SerializeField] private List<GameObject> rooms;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFloor();
    }

    /*Function spawns the first room then starts generating rooms around it.*/
    public void GenerateFloor()
    {
        SpawnFirstRoom();
        StartCoroutine(SpawnRoom(currentNumRooms));        
    }

    /*Function recursively spawns rooms adjacent to doors with no connecting rooms
     The function is a coroutine to prevent stack overflows*/
    IEnumerator SpawnRoom(int numRooms)
    {
        if (numRooms < maxNumRooms)
        {
            int roomsAdded = 0;
            for (int i = 0; i < currentNumRooms; i++) //Parse through all rooms. Does not use room.Count because rooms are added dynamically.
            {
                GameObject BaseRoom = rooms[i];
                if (BaseRoom.GetComponent<Room>().GetTopRoom() == null) //Checks if the top adjacent room is not occupied.
                {
                    //Spawns a random room from the list of possible pieces.
                    GameObject newTopRoom = Instantiate(topConPieces[Random.Range(0, topConPieces.Count)], new Vector3(BaseRoom.transform.position.x, BaseRoom.transform.position.y + yScale, transform.position.z), transform.rotation);                    
                    if (newTopRoom.GetComponent<Room>().WallCheck()) //Add the room to the list if it is a valid piece
                    {
                        rooms.Add(newTopRoom);
                        roomsAdded++; //value is incremented and added to currentNumRooms after the loop to prevent an infinite loop
                    }
                }
                if (BaseRoom.GetComponent<Room>().GetBottomRoom() == null)
                {
                    GameObject newBottomRoom = Instantiate(bottomConPieces[Random.Range(0, bottomConPieces.Count)], new Vector3(BaseRoom.transform.position.x, BaseRoom.transform.position.y - yScale, transform.position.z), transform.rotation);
                    
                    if (newBottomRoom.GetComponent<Room>().WallCheck())
                    {
                        rooms.Add(newBottomRoom);
                        roomsAdded++;
                    }
                }
                if (BaseRoom.GetComponent<Room>().GetLeftRoom() == null)
                {
                    GameObject newLeftRoom = Instantiate(leftConPieces[Random.Range(0, leftConPieces.Count)], new Vector3(BaseRoom.transform.position.x - xScale, BaseRoom.transform.position.y, transform.position.z), transform.rotation);
                    
                    if (newLeftRoom.GetComponent<Room>().WallCheck())
                    {
                        rooms.Add(newLeftRoom);
                        roomsAdded++;
                    }
                }
                if (BaseRoom.GetComponent<Room>().GetRightRoom() == null)
                {
                    GameObject newRightRoom = Instantiate(rightConPieces[Random.Range(0, rightConPieces.Count)], new Vector3(BaseRoom.transform.position.x + xScale, BaseRoom.transform.position.y, transform.position.z), transform.rotation);
                    
                    if (newRightRoom.GetComponent<Room>().WallCheck())
                    {
                        rooms.Add(newRightRoom);
                        roomsAdded++;
                    }
                }
            }
            currentNumRooms += roomsAdded;
            if (GetOpeningsCount() == 0) //Check if the room can continue to expand
            {
                CreateOpenings();
            }
            yield return new WaitForSeconds(.1f);
            StartCoroutine(SpawnRoom(currentNumRooms));
        } else
        {    
            //Remove all doors that isn't connected to a room
            for (int i = 0; i < currentNumRooms; i++)
            {
                rooms[i].GetComponent<Room>().CloseRooms();                
            }
            GameObject.Find("Character").GetComponent<CharacterMovement>().SetCurrentRoom(rooms[0]);
            GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetLimitsReady(true);
        }
    }

    /*Counts the number of doors that are not connected to a room
     RETURNS: Number of doors that are not connected to a room*/
    private int GetOpeningsCount()
    {
        int openings = 0;
        foreach (GameObject room in rooms)
        {
            openings += room.GetComponent<Room>().GetNumOpenings();
        }
        return openings;
    }

    /*Picks a random starting piece, adds the room to the list, and initializes necessary variables*/
    private void SpawnFirstRoom()
    {
        GameObject firstRoom = Instantiate(startingRooms[Random.Range(0, startingRooms.Count - 1)], transform.position, transform.rotation);
        rooms.Add(firstRoom);
        currentNumRooms = 1;
        currentNumOpenings = firstRoom.GetComponent<Room>().GetNumOpenings();
    }

    /*Destroy the last 2 rooms created so hopefully new rooms that will allow expansion will be spawned*/
    private void CreateOpenings()
    {
        for (int i = 0; i < 2; i++)
        {
            Destroy(rooms[rooms.Count - 1]);
            rooms.RemoveAt(rooms.Count - 1);
            currentNumRooms--;
        }
    }
}
