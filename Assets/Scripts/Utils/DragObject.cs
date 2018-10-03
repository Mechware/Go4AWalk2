using System.Collections;
using System.Collections.Generic;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Data.Inventory;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public bool MoveX;
	public bool MoveY;

	public Vector2 MaxBounds;
	public Vector2 MinBounds;
    public Vector2 sliderMaxBounds;
    public Vector2 sliderMinBounds;

    public float ScaleFactor;

    public UnityEvent OnDragEvent;
    public UnityEvent OnReset;
    public GameObject slider;

   // public GameEvent moving;
   // public GameEvent stopped;

	private RectTransform _rt;

	private RectTransform rt {
		get {
			if (_rt == null) _rt = GetComponent<RectTransform>();
			return _rt;
		}
	}
	private Vector3 deltaPosition;
	private Vector3 position;

	public void OnBeginDrag(PointerEventData eventData) {
        OnDragEvent.Invoke();
		eventData.Use();
	}

	public void OnDrag(PointerEventData eventData) {

 


        deltaPosition = eventData.delta;
		deltaPosition.x = !MoveX ? 0 : Mathf.RoundToInt(deltaPosition.x) * ScaleFactor;
		deltaPosition.y = !MoveY ? 0 : Mathf.RoundToInt(deltaPosition.y) * ScaleFactor;
		deltaPosition.z = 0; // Just in case.

        

        //Screen drag stuff
        if(rt.localPosition.x > 0)
            deltaPosition.y = 0;

        if (MoveY)
        {
            deltaPosition.x = 0;
            // deltaPosition.y = -deltaPosition.y; // this line inverses the drag
            if (slider != null)
            {
                float relativeLocation = (rt.localPosition.y - MinBounds.y)/(MaxBounds.y-MinBounds.y); //percentage of inventory through drag
                slider.GetComponent<RectTransform>().localPosition = new Vector2 (slider.GetComponent<RectTransform>().localPosition.x,relativeLocation*(sliderMinBounds.y-sliderMaxBounds.y)+sliderMaxBounds.y);
            }
        }

        if (rt.localPosition.x > 0 && rt.localPosition.y < 0)
             position.y = 0;        
        else
        {
            position = rt.localPosition;
            position += deltaPosition;
        }
        
		position = position.BoundVector3(MinBounds, MaxBounds);
		rt.localPosition = position;

		eventData.Use();
	}

	public void OnEndDrag(PointerEventData eventData) {
	    if (rt.localPosition.x.Equals(MinBounds.x) && rt.localPosition.y.Equals(MaxBounds.y)) { // make sure both are in the bottom corner 
            OnReset.Invoke();
	    }
        if (MoveY) OnReset.Invoke();

		eventData.Use();
	}


}
