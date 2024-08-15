using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : Health
{
    /// <summary>
    /// Health class stores entity health and deals with setting, incrementing,
    /// and fetching both current and max health.
    /// Also detects death and overheal.
    /// In theory this was meant to be shared between Player and Enemy,
    /// but I am not sure if that is a good idea or not.
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
        corpse.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Enemies/" + objectName);  // eventually should be a specific dead sprite

        corpse.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
    }
}
