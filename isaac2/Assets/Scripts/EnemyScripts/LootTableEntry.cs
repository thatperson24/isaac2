using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu] public class LootTableEntry : ScriptableObject
{
    [field: SerializeField] public string ThisResource { get; private set; }  // convert to type Resource when that class is Implemented
    [field: SerializeField] public float DropChance { get; private set; }

    public LootTableEntry(string thisResource, float dropChance)
    {
        this.ThisResource = thisResource;
        this.DropChance = dropChance;
    }
}
