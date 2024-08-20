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

    private const int resourceZLayer = -3;
    private const float resourceDropForce = 300f;

    // HELP: These should probably be private right?
    // But I can't access lootTableEntry fields in EnemyInventory methods?
    [Serializable] private struct LootTableEntry
    {
        [field: SerializeField] public GameObject Resource { get; private set; }
        [field: SerializeField] public int MinN { get; private set; }
        [field: SerializeField] public float[] DropChances { get; private set; }
    }
    [SerializeField] private List<LootTableEntry> lootTable;
    [SerializeField] private List<GameObject> inventory;
    

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
        for (int i = 0; i < n; i++)
        {
            this.inventory.Add(newResource);
        }
    }

    /// <summary>
    ///     Try to add a given Resource to Enemy Inventory based on a given
    ///     array of chances that n Resources drop.
    /// </summary>
    /// <param name="newResource"></param>
    /// <param name="dropChances"></param>
    public void AddToInventory(GameObject newResource, float[] dropChances)
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        float chanceSum = 0;
        for (int i = 0; i < dropChances.Length; i++)
        {
            float dropChance = dropChances[i];
            chanceSum += dropChance;
            if (random <= chanceSum)
            {
                AddToInventory(newResource, i);
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
        this.inventory.AddRange(newResources);
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
            AddToInventory(lootTableEntry.Resource, lootTableEntry.DropChances);
        }
    }

    /// <summary>
    ///     Return cloned List of objects in Enemy Inventory.
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetInventory()
    {
        return new List<GameObject>(this.inventory);  // Clone list
    }

    /// <summary>
    ///     Spawn contents of Enemy's inventory as GameObjects.
    ///     Resources spread randomly around Enemy corpse.
    /// </summary>
    public void SpawnLoot()
    {
        // TODO: Splash animation?
        foreach (GameObject resourcePrefab in inventory)
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
