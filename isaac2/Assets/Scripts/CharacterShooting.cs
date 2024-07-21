using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;
    public Camera cam;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCD;
    private bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

        RotateGun();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject newBullet = Instantiate(bulletPrefab, gunPivot.position, gunPivot.rotation);
            newBullet.transform.Rotate(0, 0, 0);
            float bulletSpeed = 5f; //Set using getters
            newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * newBullet.transform.right.x, bulletSpeed * newBullet.transform.right.y);

        }
        if (canShoot)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(shoot(0, 1));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(shoot(0, -1));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartCoroutine(shoot(1, 0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(shoot(-1, 0));
            }
        }
    }

    IEnumerator shoot(float xModifier, float yModifier)
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //Rotate later depending on sprite
        newBullet.transform.Rotate(0, 0, 0);
        float bulletSpeed = 5f; //Set using getters
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * xModifier, bulletSpeed * yModifier);
        canShoot = false;
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }

    void RotateGun()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 distanceVector = mousePos - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
    }
}
