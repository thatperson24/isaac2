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

    // Loot table is a List of LootTableEntry objects
    // LootTableEntry objects contain a Resource and drop chance
    // This was the only way I could think of to add a 2D data structure
    // that can be saved in a prefab and manipulated in the Editor
    // Please let me know if you have a better solution
    [SerializeField] private List<LootTableEntry> lootTable;
    [SerializeField] private List<string> inventory;  // replace type with Resource
    
    // Start is called before the first frame update
    void Start()
    {
        this.inventory = new();
        AddToInventory(lootTable);
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
    ///     Try to add a given Resource to Enemy Inventory based on a given chance,
    ///     given as a LootTableEntry object.
    /// </summary>
    /// <param name="newLootTableEntry"></param>
    public void AddToInventory(LootTableEntry newLootTableEntry)
    {
        AddToInventory(newLootTableEntry.ThisResource, newLootTableEntry.DropChance);
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
    ///     Try to add a given List of LootTableEntry objects to Enemy Inventory.
    ///     Takes into account dropChance of each resource.
    /// </summary>
    /// <param name="newLootTable"></param>
    public void AddToInventory(List<LootTableEntry> newLootTable)
    {
        foreach (var lootTableEntry in newLootTable)
        {
            AddToInventory(lootTableEntry);
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
