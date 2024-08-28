using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GridTile : MonoBehaviour
{
    public GridItem tileItem;
    public bool active;
    InventoryManager invManager;
    RectTransform UITransform;
    // Start is called before the first frame update
    void Start()
    {
        UITransform = GetComponent<RectTransform>();
        invManager = InventoryManager.inventoryManager;
    }

    public void AssignItemToTile(GridItem item)
    {
        tileItem = item;
    }

    public void ClearTile()
    {
        tileItem = null;
    }

}
