using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.UIElements;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform firingPoint;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;
    public Camera cam;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootCD;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;

    [SerializeField] private float reloadTime;
    private bool isReloading;
    private bool canShoot;

    private Vector3 distanceVector;
    [SerializeField] private float flCenteringSpeed;
    [SerializeField] private float flRecoilStr;
    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
        canShoot = true;
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        float step = flCenteringSpeed * Time.deltaTime;
        ResetFlashlight(step);
        if (!isReloading && canShoot && currentAmmo > 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                StartCoroutine(shoot());
            }
        }
        if (!isReloading && Input.GetKey(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        //Rotate later depending on sprite
        newBullet.transform.Rotate(0, 0, 0);
        float bulletSpeed = newBullet.GetComponent<Bullet>().GetBulletSpeed();
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * newBullet.transform.right.x, bulletSpeed * newBullet.transform.right.y);
        RecoilFlashlight();
        canShoot = false;
        currentAmmo--;
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }

    /* Disables shooting for <reloadTime> duration and sets current ammo to max and re-enables shooting after the duration
     * currentAmmo is reset after the timer to account for animations.
     * Parameters: N/A
     */
    IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        canShoot = true;
    }

    private void RotateGun()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        distanceVector = mousePos - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
    }

    /* Sets the camera back a certain distance depending on recoil strength
     * Parameters: N/A
     */
    private void RecoilFlashlight()
    {
        Transform flashlightTransform = this.transform.GetChild(1).transform.GetChild(0); //Getting flashlight object in heirarchy 
        float x = flashlightTransform.localPosition.x;

        flashlightTransform.localPosition = new Vector3(x - flRecoilStr, 0, 0);
    }


    /* Continuously tries to move the flashlight back to the starting position
     * Parameters:
     *  - step: the speed of the flashlight
     */
    private void ResetFlashlight(float step)
    {
        Vector2 target = new Vector2(1, 0); //original flashlight position
        Transform flashlightTransform = this.transform.GetChild(1).transform.GetChild(0);

        flashlightTransform.localPosition = Vector2.MoveTowards(flashlightTransform.localPosition, target, step);

    }
}
