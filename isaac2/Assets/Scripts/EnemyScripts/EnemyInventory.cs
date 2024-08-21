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
    ///     Depending on how backend Player Inventory is implemented,
    ///     both could inherit from an abstract Inventory class.
    ///     
    ///     This is an unfinished implementation.
    ///     Depends on future implementations of Player inventory, Resources,
    ///     and other related features.
    ///     
    ///     TODO? RemoveFromInventory?
    ///     TODO: Add % of Player Inventory to Super Enemy Inventory
    ///     TODO: Loot drop animations
    /// </summary>

    private const int resourceZLayer = -3;
    private const float resourceDropForce = 300f;

    [Serializable] private struct LootTableEntry
    {
        [field: SerializeField] public GameObject Resource { get; private set; }
        [field: SerializeField] public int MinN { get; private set; }
        [field: SerializeField] public float[] DropChances { get; private set; }
    }
    [SerializeField] private List<LootTableEntry> lootTable;

    [Serializable] private class InventoryEntry
    {
        [field: SerializeField] public GameObject Resource { get; private set; }
        [field: SerializeField] public int Count { get; set; }
        public InventoryEntry(GameObject newResource, int newCount)
        {
            this.Resource = newResource;
            this.Count = newCount;
        }
    }
    [SerializeField] private List<InventoryEntry> inventory;
    

    // Start is called before the first frame update
    void Start()
    {
        this.inventory = new();
        AddToInventory(lootTable);
    }

    /// <summary>
    ///     Add a given Resource to Enemy Inventory n times.
    /// </summary>
    /// <param name="newResource"></param>
    public void AddToInventory(GameObject newResource, int n = 1)
    {
        if (n <= 0)  // Don't add an entry
        {
            return;
        }
        for (int i = 0; i < this.inventory.Count; i++)
        {
            // If Enemy already has >= 1 of this Resource,
            // update count instead of adding new entry
            if (newResource.name.Equals(this.inventory[i].Resource.name))
            {
                this.inventory[i].Count += n;
                return;
            }
        }
        // Otherwise, add new entry
        this.inventory.Add(new InventoryEntry(newResource, n));

    }

    /// <summary>
    ///     Add a given Resource to Enemy Inventory n times,
    ///     stored as an InventoryEntry object.
    /// </summary>
    /// <param name="newResource"></param>
    private void AddToInventory(InventoryEntry newResource)
    {
        AddToInventory(newResource.Resource, newResource.Count);
    }

    /// <summary>
    ///     Try to add a given Resource to Enemy Inventory based on a given
    ///     array of chances that n Resources drop.
    /// </summary>
    /// <param name="newResource"></param>
    /// <param name="dropChances"></param>
    public void AddToInventory(GameObject newResource, float[] dropChances, int minN)
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        float chanceSum = 0;
        for (int i = 0; i < dropChances.Length; i++)
        {
            float dropChance = dropChances[i];
            chanceSum += dropChance;
            if (random <= chanceSum)
            {
                AddToInventory(newResource, i + minN);
                return;
            }
        }
    }

    /// <summary>
    ///     Add a given List of Resources to Enemy Inventory.
    /// </summary>
    /// <param name="newResources"></param>
    public void AddToInventory(List<GameObject> newResources)
    {
        foreach (GameObject resource in newResources)
        {
            AddToInventory(resource);
        }
    }

    /// <summary>
    ///     Add a given List of InventoryEntries to Enemy Inventory.
    /// </summary>
    /// <param name="newResources"></param>
    private void AddToInventory(List<InventoryEntry> newResources)
    {
        foreach (InventoryEntry resource in newResources)
        {
            AddToInventory(resource);
        }
    }

    /// <summary>
    ///     Try to add a given List of LootTableEntry structs to Enemy Inventory.
    ///     Takes into account dropChances of each Resource.
    /// </summary>
    /// <param name="newLootTable"></param>
    private void AddToInventory(List<LootTableEntry> newLootTable)
    {
        foreach (var lootTableEntry in newLootTable)
        {
            AddToInventory(lootTableEntry.Resource, lootTableEntry.DropChances, lootTableEntry.MinN);
        }
    }

    /// <summary>
    ///     Return cloned List of Resources in Enemy Inventory.
    ///     Flattened so that items with multiples appear several times,
    ///     rather than as a resource : count pair.
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetInventory()
    {
        List<GameObject> flattenedInventory = new();
        foreach (InventoryEntry entry in this.inventory)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                flattenedInventory.Add(entry.Resource);
            }
        }
        return flattenedInventory;
    }

    /// <summary>
    ///     Spawn contents of Enemy's inventory as GameObjects.
    ///     Resources spread randomly around Enemy corpse.
    /// </summary>
    public void SpawnLoot()
    {
        // TODO: Splash animation?
        foreach (GameObject resourcePrefab in this.GetInventory())
        {
            Vector3 spawnPosition = new(this.transform.position.x, this.transform.position.y, resourceZLayer);
            GameObject resourceGameObject = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
            
            // Move resource in random direction from Death point with slight force
            Vector2 dropDirection = new(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            resourceGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * resourceDropForce, ForceMode2D.Impulse);
        }
    }



    /// <summary>
    ///     Try to add a given Resource to Enemy Inventory based on a given chance
    ///     that Enemy drops one of those Resources.
    /// </summary>
    /// <param name="newResource"></param>
    /// <param name="dropChance"></param>
    //private void AddToInventory(GameObject newResource, float dropChance)
    //{
    //    float random = UnityEngine.Random.Range(0f, 1f);
    //    if (random <= dropChance)
    //    {
    //        AddToInventory(newResource);
    //    }
    //}

    /// <summary>
    ///     Try to add a given List of Resources to Enemy Inventory,
    ///     based on a given chance applied to all Resources.
    ///     Could be used by "Super" Enemies to add Player resources from last Death.
    ///     
    ///     COMMENTED OUT FOR NOW UNTIL I KNOW MORE ABOUT PLAYER INVENTORY
    /// </summary>
    /// <param name="newResources"></param>
    /// <param name="chance"></param>
    //public void AddToInventory(List<GameObject> newResources, float dropChance = 0.75f)
    //{
    //    foreach (GameObject resource in newResources)
    //    {
    //        AddToInventory(resource, chance);
    //    }
    //}
}
