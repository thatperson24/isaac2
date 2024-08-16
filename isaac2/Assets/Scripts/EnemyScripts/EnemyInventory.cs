using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyInventory : MonoBehaviour
{
    /// <summary>
    ///     Enemy Inventory Class.
    ///     Depending on how Player Inventory is implemented,
    ///     both could inherit from an abstract Inventory class.
    ///     
    ///     This is a very unfinished implementation, lots of considerations.
    ///     Just carving out some structure for future implementation so I can
    ///     also think about dropping inventory on death.
    ///     
    ///     TODO: Replace "string" with item-specific class
    /// </summary>
    
    // TODO: implement loot table
    // Dictionaries don't work with prefabs
    // Could do something ugly like two Lists
    // Could create a Loot class with Object + dropChance attributes

    [SerializeField] private List<string> inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        List<Object> inventory = new();
        // Based on loot table, randomly select inventory
        // If this is a "Super" Enemy, then also add 75% of Player's last inventory?
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

    /// <summary>
    ///     Add a given Item to Enemy Inventory.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddToInventory(string newItem)
    {
        inventory.Add(newItem);
    }

    /// <summary>
    ///     Add a given List of Objects to Enemy Inventory.
    /// </summary>
    /// <param name="newItems"></param>
    public void AddToInventory(List<string> newItems)
    {
        inventory.AddRange(newItems);
    }

    /// <summary>
    ///     Add a given percentage of a given List of Items
    ///     to the Enemy Inventory.
    ///     Could be used by "Super" Enemies to add Player resources from last Death.
    /// </summary>
    /// <param name="newItems"></param>
    /// <param name="percent"></param>
    public void AddToInventory(List<string> newItems, float percent)
    {
        // Probably very unoptimal time complexity
        // Currently grabs an exact number of items based on percent
        // Could also remove an exact number of items based on percent instead
        // Could also iterate through list, and attempt to choose each item at given % chance


        int numKept = (int) (percent * newItems.Count);
        List<string> selectedNewItems = new();
        
        for (int i = 0; i < numKept; i++)
        {
            int random = UnityEngine.Random.Range(0, newItems.Count);
            selectedNewItems.Add(newItems[random]);
            newItems.RemoveAt(random);
        }

        inventory.AddRange(selectedNewItems);
    }

    /// <summary>
    ///     Return cloned List of objects in Enemy inventory.
    /// </summary>
    /// <returns></returns>
    public List<string> GetInventory()
    {
        return new List<string>(inventory);  // clone list
    }
}
