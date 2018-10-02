using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Utils;
using G4AW2.Data.Inventory;

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
           
            if (TransformToCanvas.isBounded(position, inventory, playerReference,dragOffset.y))
            {
                Equip.Raise(gameObject.GetComponentInChildren<ItemDisplay>().getItem());
                unEquip.Raise(gameObject);

                //playerReference.GetComponent<EquipDisplay>().EquipItem(gameObject);
            }
        } else
        {
            if (!TransformToCanvas.isBounded(position, inventory.rect))
            {
                Equip.Raise(gameObject.GetComponentInChildren<ItemDisplay>().getItem());
            }
        }

        //testing itemremove
        
        //if(onPlayer)
       // GetComponentInParent<InventoryDisplay>().removeItem(gameObject);

      }


    private int nearestGridIndex(Vector3 pos)
    {
        Vector3 grid = new Vector3();
        if (TransformToCanvas.isBounded(position, inventory.rect))
        {
            grid = position - gridStart;
            grid.x = grid.x/deltaGrid.x;
            grid.y = grid.y/deltaGrid.y;

            int x = Mathf.RoundToInt(grid.x);
            int y = Mathf.RoundToInt(grid.y);

            return y*GetComponentInParent<InventoryDisplay>().columns + x;
        } else return -1;

    }

    public void assignGrid(Vector3 start, Vector3 add)
    {
        gridStart = start;
        deltaGrid = add;
    }



    /* void OnTriggerEnter2D(Collider2D col)
     {
         if (col.tag == "Player")
         {
             onPlayer=true;
             Debug.Log("overlapping player");
         }

     }

     void OnTriggerExit2D(Collider2D col)
     {
         if (col.tag == "Player")
         {
             onPlayer = false;
             Debug.Log("not overlapping player");
         }

     }*/





}
