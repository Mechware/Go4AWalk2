using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Utils;


public class DragItem : MonoBehaviour, IDragHandler,IEndDragHandler {

    private RectTransform _rt;
    public RectTransform inventory;
    private Vector2 startPosition, position;
    public Camera cam;
 

    private RectTransform rt
    {
        get
        {
            if (_rt == null)
            {
                _rt = GetComponent<RectTransform>();

                startPosition = _rt.position;

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

      }

    public void OnEndDrag(PointerEventData eventData)
      {
        rt.position = startPosition;
        rt.GetComponent<Canvas>().sortingOrder = 0;
        eventData.Use();
      }


}
