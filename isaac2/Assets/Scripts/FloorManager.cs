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
    [SerializeField] private List<GameObject> topPieces;
    [SerializeField] private List<GameObject> bottomPieces;
    [SerializeField] private List<GameObject> leftPieces;
    [SerializeField] private List<GameObject> rightPieces;

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
        currentNumOpenings = firstRoom.GetComponent<Room>().GetEmptyNumOpenings();

        
        while (currentNumRooms < maxNumRooms)
        {
            int roomsAdded = 0;
            for (int i = 0; i < currentNumRooms; i++)
            {
                if (rooms[i].GetComponent<Room>().GetTopRoom() == null)
                {
                    GameObject newTopRoom = Instantiate(topPieces[Random.Range(0, topPieces.Count - 1)], new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y + yScale, transform.position.z), transform.rotation);
                    newTopRoom.name = "" + newTopRoom.transform.position.x + " - " + newTopRoom.transform.position.y;
                    rooms.Add(newTopRoom);
                    roomsAdded++;
                }
                if (rooms[i].GetComponent<Room>().GetBottomRoom() == null)
                {
                    GameObject newBottomRoom = Instantiate(topPieces[Random.Range(0, topPieces.Count - 1)], new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.y - yScale, transform.position.z), transform.rotation);
                    newBottomRoom.name = "" + newBottomRoom.transform.position.x + " - " + newBottomRoom.transform.position.y;
                    rooms.Add(newBottomRoom);
                    roomsAdded++;
                }
                if (rooms[i].GetComponent<Room>().GetLeftRoom() == null)
                {
                    GameObject newLeftRoom = Instantiate(topPieces[Random.Range(0, topPieces.Count - 1)], new Vector3(rooms[i].transform.position.x - xScale, rooms[i].transform.position.y, transform.position.z), transform.rotation);
                    newLeftRoom.name = "" + newLeftRoom.transform.position.x + " - " + newLeftRoom.transform.position.y;
                    rooms.Add(newLeftRoom);
                    roomsAdded++;
                }
                if (rooms[i].GetComponent<Room>().GetRightRoom() == null)
                {
                    GameObject newRightRoom = Instantiate(topPieces[Random.Range(0, topPieces.Count - 1)], new Vector3(rooms[i].transform.position.x + xScale, rooms[i].transform.position.y, transform.position.z), transform.rotation);
                    newRightRoom.name = "" + newRightRoom.transform.position.x + " - " + newRightRoom.transform.position.y;
                    rooms.Add(newRightRoom);
                    roomsAdded++;
                }
            }
            currentNumRooms += roomsAdded;
        }
    }
}
