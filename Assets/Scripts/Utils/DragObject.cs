using System.Collections;
using System.Collections.Generic;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public bool MoveX;
	public bool MoveY;

	public Vector2 MaxBounds;
	public Vector2 MinBounds;

    public float ScaleFactor;

    public UnityEvent OnDragEvent;
    public UnityEvent OnReset;

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

        if(rt.localPosition.y < 0)
            deltaPosition.x = 0;

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
		eventData.Use();
	}
}
