using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Utils;
using G4AW2.Data.Inventory;
using G4AW2.Data.DropSystem;

public enum InventoryType
{
    basic = 0,
    eqiupped = 1
}


public class DragItem : MonoBehaviour, IDragHandler,IEndDragHandler,IBeginDragHandler {

    private RectTransform _rt;
    public RectTransform inventory,playerReference,dragScreen;
    private Vector2 startPosition, position, gridStart, deltaGrid, dragOffset;
    public Camera cam;
    private bool onPlayer;
    public GameEventGameObject unEquip;
    public GameEventItem Equip;
    
    public InventoryType inventoryType;
 

    private RectTransform rt
    {
        get
        {
            if (_rt == null)
            {
                _rt = GetComponent<RectTransform>();

               // startPosition = _rt.anchoredPosition;

            }
            return _rt;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        dragOffset = new Vector2(0, 0);
        startPosition = rt.anchoredPosition;
        rt.GetComponent<Canvas>().overrideSorting = true;
        rt.GetComponent<Canvas>().sortingOrder += 4;
    }

    public void OnDrag(PointerEventData eventData)
      {

        position = eventData.position;
        position = cam.ScreenToWorldPoint(eventData.position);
        position = TransformToCanvas.Transform(position, inventory);
        if (dragScreen != null)
        {
            dragOffset.y = -(dragScreen.anchoredPosition.y-dragScreen.GetComponent<InventoryDisplay>().startCenter().y);
            position += dragOffset;
            Debug.Log(dragOffset + "  :  " + dragScreen.GetComponent<InventoryDisplay>().startCenter().y + "    :    " + dragScreen.anchoredPosition);
        }

        rt.anchoredPosition = position;

        Debug.Log(position);

        eventData.Use();
        if (inventoryType==InventoryType.basic) 
        Debug.Log("index: " + nearestGridIndex(position));
       /* Debug.Log("Position: " + position + "    :    is bounded?: " + TransformToCanvas.isBounded(position, TransformToCanvas.BoundingRectangle(inventory, playerReference)) +
            "\nRect: " + TransformToCanvas.convert(TransformToCanvas.BoundingRectangle(inventory, playerReference)));*/

      }

    public void OnEndDrag(PointerEventData eventData)
      {
        Debug.Log(position);
        rt.anchoredPosition = startPosition;
        rt.GetComponent<Canvas>().overrideSorting = false;
        rt.GetComponent<Canvas>().sortingOrder -= 4;
        eventData.Use();

        if (inventoryType == InventoryType.basic)
        {

            GetComponentInParent<InventoryDisplay>().moveItem(gameObject, nearestGridIndex(position));
           
            if (TransformToCanvas.isBounded(position, inventory, playerReference,dragOffset.y) && gameObject.GetComponentInChildren<ItemDisplay>().getItem().type != ItemType.Consumable && gameObject.GetComponentInChildren<ItemDisplay>().getItem().type != ItemType.Material)
            {
                Equip.Raise(gameObject.GetComponentInChildren<ItemDisplay>().getItem());
                unEquip.Raise(gameObject);
            } //Add case for using consumable objects the same way as equipping something later
        } else
        {
            if (!TransformToCanvas.isBounded(position, inventory.rect))
            {
                Equip.Raise(gameObject.GetComponentInChildren<ItemDisplay>().getItem());
            }
        }

      }


    private int nearestGridIndex(Vector3 pos)
    {
        gridStart = dragScreen.GetComponent<InventoryDisplay>().getGrid()[0];
        deltaGrid = dragScreen.GetComponent<InventoryDisplay>().getGrid()[1];
        Vector3 grid = new Vector3();
        if (TransformToCanvas.isBounded(position, dragScreen.rect))
        {
            grid = position - gridStart;
            grid.x = grid.x/deltaGrid.x;
            grid.y = grid.y/deltaGrid.y;

            int x = Mathf.RoundToInt(grid.x);
            int y = Mathf.RoundToInt(grid.y);

            return y*GetComponentInParent<InventoryDisplay>().columns + x;
        } else return -1;

    }


}
