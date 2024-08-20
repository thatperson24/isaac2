using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform topLimit;
    [SerializeField] private Transform bottomLimit;

    [SerializeField] private Camera camera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float transitionSpeed;

    [SerializeField] private Transform gunPivot;
    private bool limitsReady;
    private bool isTransitioning;
    private float[] transitionDir;
    // Start is called before the first frame update
    void Start()
    {
        limitsReady = false;
        isTransitioning = false;
        transitionDir = new float[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (limitsReady)
        {
            float vertical = camera.orthographicSize;
            float horizontal = vertical * Screen.width / Screen.height;
            Vector3 position = transform.position;

            float deltax = 0;
            float deltay = 0;

            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distanceVector = mousePos - gunPivot.position;

            position.x = Mathf.Clamp(Mathf.Clamp(position.x + deltax + (distanceVector.x * .001f), transform.parent.position.x - 2f, transform.parent.position.x + 2f), leftLimit.position.x + horizontal, rightLimit.position.x - horizontal);
            position.y = Mathf.Clamp(Mathf.Clamp(position.y + deltay + (distanceVector.y * .001f), transform.parent.position.y - 2f, transform.parent.position.y + 2f), bottomLimit.position.y + vertical, topLimit.position.y - vertical);

            transform.position = position;
        }
        if(isTransitioning)
        {
            Vector3 position = transform.position;
            position.x += transitionDir[0] * Time.deltaTime * transitionSpeed;
            position.y += transitionDir[1] * Time.deltaTime * transitionSpeed * 9/16f;
            transform.position = position;
        }
    }

    public void SetTop(Transform limit)
    {
        topLimit = limit;
    }

    public void SetBottom(Transform limit)
    {
        bottomLimit = limit;
    }

    public void SetLeft(Transform limit)
    {
        leftLimit = limit;
    }

    public void SetRight(Transform limit)
    {
        rightLimit = limit;
    }

    public void SetLimitsReady(bool val)
    {
        limitsReady = val;
    }

    public void SetIsTransitioning(bool val)
    {
        isTransitioning = val; 
    }

    public void SetTransitionDir(int x, int y)
    {
        transitionDir[0] = x;
        transitionDir[1] = y;
    }

    public void RecoilCamera()
    {
        float vertical = camera.orthographicSize;
        float horizontal = vertical * Screen.width / Screen.height;
        Vector3 position = transform.position;

        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceVector = mousePos - gunPivot.position;

        position.x = Mathf.Clamp(Mathf.Clamp(position.x + (-distanceVector.x * .1f), transform.parent.position.x - 2f, transform.parent.position.x + 2f), leftLimit.position.x + horizontal, rightLimit.position.x - horizontal);
        position.y = Mathf.Clamp(Mathf.Clamp(position.y + (-distanceVector.y * .1f), transform.parent.position.y - 2f, transform.parent.position.y + 2f), bottomLimit.position.y + vertical, topLimit.position.y - vertical);

        transform.position = position;
    }
}
