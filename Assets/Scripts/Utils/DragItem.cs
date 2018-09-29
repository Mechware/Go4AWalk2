using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Utils;
using G4AW2.Data.Inventory;

public class DragItem : MonoBehaviour, IDragHandler,IEndDragHandler,IBeginDragHandler {

    private RectTransform _rt;
    public RectTransform inventory,playerReference;
    private Vector2 startPosition, position, gridStart, deltaGrid;
    public Camera cam;
    private bool onPlayer;
 

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
        startPosition = rt.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
      {

        position = eventData.position;
        position = cam.ScreenToWorldPoint(eventData.position);
        position = TransformToCanvas.Transform(position, inventory);
        rt.GetComponent<Canvas>().sortingOrder = 1;
        rt.anchoredPosition = position;
        eventData.Use();
        Debug.Log("index: " + nearestGridIndex(position));
       /* Debug.Log("Position: " + position + "    :    is bounded?: " + TransformToCanvas.isBounded(position, TransformToCanvas.BoundingRectangle(inventory, playerReference)) +
            "\nRect: " + TransformToCanvas.convert(TransformToCanvas.BoundingRectangle(inventory, playerReference)));*/

      }

    public void OnEndDrag(PointerEventData eventData)
      {
        rt.anchoredPosition = startPosition;
        rt.GetComponent<Canvas>().sortingOrder = 0;
        eventData.Use();

        GetComponentInParent<InventoryDisplay>().moveItem(gameObject, nearestGridIndex(position));

        if (TransformToCanvas.isBounded(position, inventory, playerReference)) Debug.Log("equipped");

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
