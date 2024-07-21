using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private int xScale;
    [SerializeField] private int yScale;

    [SerializeField] private int maxNumRooms;
    [SerializeField] private int currentNumRooms;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateFloor()
    {
        GameObject firstRoom = Instantiate(startingRooms[Random.Range(0,startingRooms.Count - 1)],transform.position, transform.rotation);
        firstRoom.name = "" + firstRoom.transform.position.x + " - " + firstRoom.transform.position.y;
        rooms.Add(firstRoom);
        currentNumRooms = 1;
        currentNumOpenings = firstRoom.GetComponent<Room>().GetNumOpenings();

        
        while (currentNumRooms < maxNumRooms)
        {
            int roomsAdded = 0;
            for (int i = 0; i < currentNumRooms; i++)
            {
                roomsAdded += SpawnRoom(rooms[i], false);
                
            }
            currentNumRooms += roomsAdded;
        }

        Debug.Log("");
        for (int i = 0; i < currentNumRooms; i++)
        {        
            rooms[i].GetComponent<Room>().CloseRooms();
        }
        
    }

    private int SpawnRoom(GameObject BaseRoom, bool destroyRoom)
    {
        int roomsAdded = 0;
        if (BaseRoom.GetComponent<Room>().GetTopRoom() == null)
        {
            GameObject newTopRoom = Instantiate(topConPieces[Random.Range(0, topConPieces.Count)], new Vector3(BaseRoom.transform.position.x, BaseRoom.transform.position.y + yScale, transform.position.z), transform.rotation);
            bool spawned = newTopRoom.GetComponent<Room>().WallCheck(destroyRoom);
            if (spawned)
            {
                rooms.Add(newTopRoom);
                roomsAdded ++;
            }
        }
        if (BaseRoom.GetComponent<Room>().GetBottomRoom() == null)
        {
            GameObject newBottomRoom = Instantiate(bottomConPieces[Random.Range(0, bottomConPieces.Count)], new Vector3(BaseRoom.transform.position.x, BaseRoom.transform.position.y - yScale, transform.position.z), transform.rotation);
            bool spawned = newBottomRoom.GetComponent<Room>().WallCheck(destroyRoom);
            if (spawned) {
                rooms.Add(newBottomRoom);
                roomsAdded++;
            }
        }
        if (BaseRoom.GetComponent<Room>().GetLeftRoom() == null)
        {
            GameObject newLeftRoom = Instantiate(leftConPieces[Random.Range(0, leftConPieces.Count)], new Vector3(BaseRoom.transform.position.x - xScale, BaseRoom.transform.position.y, transform.position.z), transform.rotation);
            bool spawned = newLeftRoom.GetComponent<Room>().WallCheck(destroyRoom);
            if (spawned)
            {
                rooms.Add(newLeftRoom);
                roomsAdded++;
            }
        }
        if (BaseRoom.GetComponent<Room>().GetRightRoom() == null)
        {
            GameObject newRightRoom = Instantiate(rightConPieces[Random.Range(0, rightConPieces.Count)], new Vector3(BaseRoom.transform.position.x + xScale, BaseRoom.transform.position.y, transform.position.z), transform.rotation);
            bool spawned = newRightRoom.GetComponent<Room>().WallCheck(destroyRoom);
            if (spawned)
            {
                rooms.Add(newRightRoom);
                roomsAdded++;
            }
        }
        return roomsAdded;
    }
}
