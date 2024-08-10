using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.Collections;
using Unity.VisualScripting;
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

    // Bullet is trigger collider, damages Enemy when hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On spawn, Bullet collides with Player - want to ignore this
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyHealth>().Damage(); // HP - 1
            }
            Destroy(gameObject);  // Destroy Bullet after collision
        }
    }
}
