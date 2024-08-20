using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : Health
{
    /// <summary>
    ///     Enemy implentation of Health abstract class, 
    ///     which handles Entity max & cur health, damage,
    ///     healing, death, etc.
    ///     Implements Death() method, which handles Enemy death.
    ///     Also handles corpse spawning, (future) death animation,
    ///     and (future) loot dropping.
    /// </summary> 

    [SerializeField] private GameObject resourcePrefab;

    /// <summary>
    ///     Initiate Enemy death process.
    /// </summary>
    public override void Death()
    {
        this.GetComponent<EnemyPursue>().SetCurSpeed(0);
        // TODO: Death animation
        // this.GetComponent<EnemyAttack>().DeathAttack();  // Any Attacks/effects on Death
        SpawnCorpse();
        Destroy(this.gameObject);  // , delay);  // Delay by length of Death animation
        // TODO: Death effects / Attacks (i.e., explosion, lingering area effect, etc.)
        this.GetComponent<EnemyInventory>().SpawnLoot();
    }

    /// <summary>
    ///     Spawn an AI-less corpse entity with the correct
    ///     sprite for this Enemy.
    /// </summary>
    private void SpawnCorpse()
    {
        String objectName = this.gameObject.name;
        if (objectName[^7..].Equals("(Clone)"))
        {
            objectName = objectName[..^7];
        }
        
        GameObject corpse = new(objectName + "Corpse");
        corpse.AddComponent<SpriteRenderer>();
        corpse.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Enemies/" + objectName);
        // Eventually should be a specific dead sprite, maybe correct orientation as well

        corpse.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }
}
