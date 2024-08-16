using System;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    ///     Initiate Enemy death process.
    /// </summary>
    public override void Death()
    {
        this.GetComponent<EnemyPursue>().SetCurSpeed(0);
        // TODO: death animation
        SpawnCorpse();
        Destroy(this.gameObject);  // , delay);  // delay by length of death animation
        SpawnLoot();
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
        // eventually should be a specific dead sprite, maybe correct orientation as well

        corpse.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    /// <summary>
    ///     This will eventually handle Enemy dropping loot on Death.
    ///     Lots of possibilities here, depends on how we handle Items,
    ///     Enemy Inventory, Player Inventory, etc.
    /// </summary>
    private void SpawnLoot()
    {
        List<string> inventory = this.GetComponent<EnemyInventory>().GetInventory();
        foreach (string item in inventory)
        {
            Debug.Log(item);
            // Spawn Items
            // Could be held by corpse and interacting with corpse puts them in inventory?
            // Could be scattered randomly within a range around corpse?
            // Could be immediately placed in Player inventory
        }
    }
}
