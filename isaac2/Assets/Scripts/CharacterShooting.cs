using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
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
}
