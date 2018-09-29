using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Utils;
using G4AW2.Data.Inventory;

public class DragItem : MonoBehaviour, IDragHandler,IEndDragHandler {

    private RectTransform _rt;
    public RectTransform inventory,playerReference;
    private Vector2 startPosition, position;
    public Camera cam;
    private bool onPlayer;
 

    private RectTransform rt
    {
        get
        {
            if (_rt == null)
            {
                _rt = GetComponent<RectTransform>();

                startPosition = _rt.anchoredPosition;

            }
            return _rt;
        }
    }

    public void OnDrag(PointerEventData eventData)
      {
        position = eventData.position;
        position = cam.ScreenToWorldPoint(eventData.position);
        position = TransformToCanvas.Transform(position, inventory);
        rt.GetComponent<Canvas>().sortingOrder = 1;
        rt.anchoredPosition = position;
        eventData.Use();

       // Debug.Log("Position: " + position + "    :    is bounded?: " + VectorUtils.isBounded(position, TransformToCanvas.BoundingRectangle(inventory, playerReference)) +
       //     "\nRect: " + TransformToCanvas.BoundingRectangle(inventory, playerReference));

      }

    public void OnEndDrag(PointerEventData eventData)
      {
        rt.anchoredPosition = startPosition;
        rt.GetComponent<Canvas>().sortingOrder = 0;
        eventData.Use();




        //testing itemremove
        //if (VectorUtils.isBounded(position, TransformToCanvas.BoundingRectangle(inventory, playerReference)))
        if(onPlayer)
        GetComponentInParent<InventoryDisplay>().removeItem(gameObject);

      }


    void OnTriggerEnter2D(Collider2D col)
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

    }

    



}
