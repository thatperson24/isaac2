using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyInventory : MonoBehaviour
{
    /// <summary>
    ///     Handles Enemy Inventory.
    ///     Stores Inventory as a list of Resources (strings for now).
    ///     Enemy also has a Loot Table with Resources and drop chances.
    ///     
    ///     Depending on how Player Inventory is implemented,
    ///     both could inherit from an abstract Inventory class.
    ///     
    ///     This is a very unfinished implementation, lots of considerations.
    ///     Just carving out some structure for future implementation so I can
    ///     also think about dropping inventory on death.
    ///     
    ///     TODO: Replace "string" with Resource class
    /// </summary>

    // I would like to replace this with a serializable dictionary
    // or other data structure that can be manipulated in the editor,
    // or I guess a loot table
    [SerializeField] private Dictionary<string, float> lootTable;
    [SerializeField] private List<string> inventory;  // replace type with Resource
    
    // Start is called before the first frame update
    void Start()
    {
        this.inventory = new();
        TempInitLootTable();
        AddToInventory(lootTable);
    }

    // either could replace dictionary with a serializable data type
    // or store loot table as csv/text/etc and read into dictionary?
    private void TempInitLootTable()
    {
        lootTable = new Dictionary<string, float> { {"silver", 1f }, {"gold", 0.5f } };
    }

    /// <summary>
    ///     Add a given Resource to Enemy Inventory.
    /// </summary>
    /// <param name="newResource"></param>
    public void AddToInventory(string newResource)
    {
        this.inventory.Add(newResource);
    }

    /// <summary>
    ///     Try to add a given Resource to Enemy Inventory based on a given chance.
    /// </summary>
    /// <param name="newResource"></param>
    /// <param name="chance"></param>
    public void AddToInventory(string newResource, float chance)
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        if (random <= chance)
        {
            AddToInventory(newResource);
        }
    }

    /// <summary>
    ///     Add a given List of Resources to Enemy Inventory.
    /// </summary>
    /// <param name="newResources"></param>
    public void AddToInventory(List<string> newResources)
    {
        this.inventory.AddRange(newResources);
    }

    /// <summary>
    ///     Try to add a given List of Resources to Enemy Inventory,
    ///     based on a given chance applied to all Resources.
    ///     Could be used by "Super" Enemies to add Player resources from last Death.
    /// </summary>
    /// <param name="newResources"></param>
    /// <param name="chance"></param>
    public void AddToInventory(List<string> newResources, float chance = 0.75f)
    {
        foreach (var resource in newResources)
        {
            AddToInventory(resource, chance);
        }
    }

    /// <summary>
    ///     Try to add a given Dictionary of Resources to Enemy Inventory.
    ///     Takes into account dropChance of each resource.
    /// </summary>
    /// <param name="newLootTable"></param>
    public void AddToInventory(Dictionary<string, float> newLootTable)
    {
        foreach (var lootTableEntry in newLootTable)
        {
            AddToInventory(lootTableEntry.Key, lootTableEntry.Value);
        }
    }

    /// <summary>
    ///     Return cloned List of objects in Enemy inventory.
    /// </summary>
    /// <returns></returns>
    public List<string> GetInventory()
    {
        return new List<string>(this.inventory);  // clone list
    }
}
