using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject character;
    private bool controlsEnabled;

    private GameObject currentRoom;
    private GameObject currentDoor;

    private void Start()
    {
        controlsEnabled = true;
    }
    void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        if (controlsEnabled)
        {
            //Horizontal movement
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-1f * speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Vertical movement
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector2(rb.velocity.x, -1f * speed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            //Interact button
            if (Input.GetKey(KeyCode.E))
            {
                EnterDoor();
            }
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = character.transform.localScale;
            localScale.x *= -1f;
            character.transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDoorCheck") && collision.gameObject.transform.parent.GetComponent<Door>().GetDoorActive())
        {
            collision.gameObject.transform.parent.GetComponent<Door>().SpawnButtonPrompt(); //Spawns button prompt to enter door
            currentDoor = collision.gameObject.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDoorCheck") && collision.gameObject.transform.parent.GetComponent<Door>().GetDoorActive())
        {
            Destroy(collision.gameObject.transform.parent.transform.GetChild(2).gameObject); //Despawns button prompt to enter door.
            currentDoor = null;
        }
    }

    private void EnterDoor()
    {
        if (currentDoor != null)
        {
            GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetLimitsReady(false);
            controlsEnabled = false;
            Vector2 direction = new Vector2();
            switch(currentDoor.GetComponent<Door>().GetDoorLocation())
            {
                case Door.DOOR.TOP:
                    SetCurrentRoom(currentRoom.GetComponent<Room>().GetTopRoom());
                    direction = new Vector2(0,1);
                    GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTransitionDir(0, 1);
                    break;
                case Door.DOOR.BOTTOM:
                    SetCurrentRoom(currentRoom.GetComponent<Room>().GetBottomRoom());
                    direction = new Vector2(0, -1);
                    GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTransitionDir(0, -1);
                    break;
                case Door.DOOR.LEFT:
                    SetCurrentRoom(currentRoom.GetComponent<Room>().GetLeftRoom());
                    direction = new Vector2(-1, 0);
                    GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTransitionDir(-1, 0);
                    break;
                case Door.DOOR.RIGHT:
                    SetCurrentRoom(currentRoom.GetComponent<Room>().GetRightRoom());
                    direction = new Vector2(1, 0);
                    GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTransitionDir(1,0);
                    break;
            }
            StartCoroutine(TransitionRooms(direction));
        }
    }

    public void SetCurrentRoom(GameObject room)
    {
        currentRoom = room;
        SetCameraLimits(currentRoom);
    }

    private void SetCameraLimits(GameObject room)
    {
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTop(room.transform.GetChild(1).transform.GetChild(0).transform);
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetBottom(room.transform.GetChild(1).transform.GetChild(1).transform);
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetLeft(room.transform.GetChild(1).transform.GetChild(2).transform);
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetRight(room.transform.GetChild(1).transform.GetChild(3).transform);
    }

    IEnumerator TransitionRooms(Vector2 direction)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = direction*speed;
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetIsTransitioning(true);
        yield return new WaitForSeconds(1.33f);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        controlsEnabled = true;
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetLimitsReady(true);
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetIsTransitioning(false);
        GameObject.Find("CameraHolder").GetComponent<CameraMovement>().SetTransitionDir(0,0);
    }
}

