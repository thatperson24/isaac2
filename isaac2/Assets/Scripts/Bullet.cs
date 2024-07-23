using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeSpan;
    void Awake()
    {
        StartCoroutine(BulletLifeSpan());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    IEnumerator BulletLifeSpan()
    {
        yield return new WaitForSeconds(bulletLifeSpan);
        Destroy(this.gameObject);
    }
}
