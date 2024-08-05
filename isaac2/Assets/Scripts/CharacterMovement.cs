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
    void Update()
    {
        Flip();
    }

    private void FixedUpdate()
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDoorCheck") && collision.gameObject.transform.parent.GetComponent<Door>().GetDoorActive())
        {
            Destroy(collision.gameObject.transform.parent.transform.GetChild(2).gameObject); //Despawns button prompt to enter door.
        }
    }
}
