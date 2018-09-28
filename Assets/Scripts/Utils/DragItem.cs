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
    public FloatVariable itemWidth;
    private float scaleFactor;
    public Camera cam;
 

    private RectTransform rt
    {
        get
        {
            if (_rt == null)
            {
                _rt = GetComponent<RectTransform>();
                scaleFactor = _rt.localScale.x;
                startPosition = _rt.position;

            }
            return _rt;
        }
    }

/*    private Vector2 WorldBottomLeft(RectTransform rt)
    {
        Vector2 center = new Vector2(0, 0);
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return corners[0];
    }

    private Vector2 LocalBottomLeft(RectTransform rt)
    {
        Vector2 center = new Vector2(0, 0);
        Vector3[] corners = new Vector3[4];
        rt.GetLocalCorners(corners);
        return corners[0];
    }

    private Vector2 WorldCenter(RectTransform rt)
    {
        Vector2 center = new Vector2(0, 0);
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        center.y = (corners[1].y+corners[0].y)/2;
        center.x = (corners[2].x+corners[1].x)/2;
        return center;
    }

    private Vector2 GetTransformScale(Vector2 worldCenter, Vector2 worldBottomLeft, Vector2 localBottomLeft)
    {
        Vector2 scale = new Vector2(0,0);

        scale.x = localBottomLeft.x/(worldCenter.x-worldBottomLeft.x);
        scale.y = localBottomLeft.y/(worldCenter.y-worldBottomLeft.y);

        return scale;
    }*/


    public void OnDrag(PointerEventData eventData)
      {
        //Vector2 center = WorldCenter(inventory);
        position = eventData.position;
        position = cam.ScreenToWorldPoint(eventData.position);
        //Debug.Log("position before " + position + "    :    center " + center + "    :    localCorner " + LocalBottomLeft(inventory) + "    :    WorldCorner " + WorldBottomLeft(inventory));
        //Vector2 scale = GetTransformScale(center, WorldBottomLeft(inventory), LocalBottomLeft(inventory));
        //position.x = (center.x-position.x)*scale.x -itemWidth*scaleFactor/2;
        //position.y = (center.y-position.y)*scale.y -itemWidth*scaleFactor/2;
        position = TransformToCanvas.Transform(position, inventory);
        //position.x = position.x-itemWidth*scaleFactor/2;
        //position.y = position.y-itemWidth*scaleFactor/2;
        rt.GetComponent<Canvas>().sortingOrder = 1;
        rt.anchoredPosition = position;
        //Debug.Log("position before " + cam.ScreenToWorldPoint(eventData.position) + "    :    position after " + position + "    :    Scale " + scale + "\ncenter " + center + "    :    localCorner " + LocalBottomLeft(inventory) + "    :    WorldCorner " + WorldBottomLeft(inventory));
        eventData.Use();

      }

    public void OnEndDrag(PointerEventData eventData)
      {
          rt.position = startPosition;
        rt.GetComponent<Canvas>().sortingOrder = 0;
        eventData.Use();
      }


}
