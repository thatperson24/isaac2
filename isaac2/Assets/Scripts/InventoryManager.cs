using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    [SerializeField]
    GridItem mouseItem;
    [SerializeField]
    GridTile mouseTile;
    // Start is called before the first frame update
    void Awake()
    {
        if (inventoryManager != null && inventoryManager != this)
        {
            Destroy(this);
        }
        else
        {
            inventoryManager = this;
        }

    }

    public void SelectItem(GridItem item)
    {
        if (item == mouseItem)
        {
            mouseItem = null;
            return;
        }

        mouseItem = item;
    }

    public void DropItem()
    {
        if (!mouseTile)
        {
            mouseItem.ResetPosition();

            List<GridTile> mouseItemTiles = mouseItem.GetNumberOfTilesOccupied();
            if(mouseItemTiles.Count != 0)
            {
                mouseItem.ClearTilesOccupied();
            }
        }
        else
        {
            mouseItem.transform.position = mouseTile.transform.position;

        }
        mouseItem = null;


    }
    public void UpdateHoveredTile(GridTile tile)
    {
        mouseTile = tile;
    }
}
