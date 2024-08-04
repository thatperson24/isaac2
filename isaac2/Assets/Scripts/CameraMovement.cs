using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform topLimit;
    [SerializeField] private Transform bottomLimit;

    [SerializeField] private Camera camera;
    [SerializeField] private float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = camera.orthographicSize;
        float horizontal = vertical * Screen.width / Screen.height;
        Vector3 position = transform.position;

        float deltax = 0;
        float deltay = 0;
        //Try to recenter camera horizontally when moving away from the wall
        if(transform.localPosition.x > 0.01f && position.x != leftLimit.position.x + horizontal)
        {
            deltax = -cameraSpeed * Time.deltaTime * .5f;
        } 
        else if(transform.localPosition.x < -0.01f && position.x != rightLimit.position.x - horizontal)
        {
            deltax = cameraSpeed * Time.deltaTime * .5f;
        }
        position.x = Mathf.Clamp(position.x + deltax, leftLimit.position.x + horizontal, rightLimit.position.x - horizontal);

        //Try to recenter camera vertically when moving away from the wall
        if (transform.localPosition.y > 0.01f && position.y != bottomLimit.position.y + vertical)
        {
            deltay = -cameraSpeed * Time.deltaTime * .5f;
        }
        else if (transform.localPosition.y < -0.01f && position.y != topLimit.position.y - vertical)
        {
            deltax = cameraSpeed * Time.deltaTime * .5f;
        }
        position.y = Mathf.Clamp(position.y + deltay, bottomLimit.position.y + vertical, topLimit.position.y - vertical);

        transform.position = position;


    }
}
