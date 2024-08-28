using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 originalPosition;
    public Canvas parentCanvas;
    public int gridSlots;
    bool dragging;
    InventoryManager invManager;
    List<GridTile> tilesOccupied;
    // Start is called before the first frame update
    void Start()
    {
        tilesOccupied = new List<GridTile>();
        invManager = InventoryManager.inventoryManager;
        originalPosition = transform.position;
        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging)
        {

            // if (transform.position != originalPosition)
            // {
            //     transform.position = originalPosition;
            // }

            return;
        }

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera, out mousePos
        );

        transform.position = parentCanvas.transform.TransformPoint(mousePos);

        var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        // Debug.Log(pointerEventData.position);
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        bool foundTile = false;
        foreach (RaycastResult raycastResult in raycastResults)
        {
            GridTile hitTile = raycastResult.gameObject.GetComponent<GridTile>();
            if (hitTile != null)
            {
                invManager.UpdateHoveredTile(hitTile);
                foundTile = true;
                break;
            }
        }

        if (!foundTile)
        {
            invManager.UpdateHoveredTile(null);
        }
        Debug.Log(raycastResults.Count);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("mouse down on " + gameObject.name);
        invManager.DropItem();
        // transform.parent = parentObject;
        dragging = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("mouse down on " + gameObject.name);
        invManager.SelectItem(this);

    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        // transform.parent = null;
        dragging = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // GetComponent<Image>().raycastTarget = false;
        Debug.Log("Started Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // GetComponent<Image>().raycastTarget = true;
        Debug.Log("End Drag");

    }

    public void ResetPosition()
    {
        transform.position = originalPosition;

    }

    public void StoreItem(GridTile newTile)
    {
        tilesOccupied.Add(newTile);
    }

    public List<GridTile> GetNumberOfTilesOccupied()
    {
        return tilesOccupied;
    }

    public void ClearTilesOccupied()
    {
        foreach(GridTile tile in tilesOccupied)
        {
            tile.ClearTile();
        }
        tilesOccupied.Clear();
    }
}
