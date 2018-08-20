using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackArea : Graphic, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public GameEvent OnTap;
    public GameEventVector3Arr OnSwipe;

	public LineRenderer LineRenderer;

	protected override void Awake() {
		color = new Color(0, 0, 0, 0);
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (dragging) return;
		OnTap.Raise();
        eventData.Use();
	}

	private bool dragging = false;
	public void OnBeginDrag(PointerEventData eventData) {
		dragging = true;
		LineRenderer.positionCount = 0;
		eventData.Use();
	}

	public void OnEndDrag(PointerEventData eventData) {
		dragging = false;
		LineRenderer.Simplify(20);
		Vector3[] points = new Vector3[LineRenderer.positionCount];
		LineRenderer.GetPositions(points);
	    OnSwipe.Raise(points);
        LineRenderer.positionCount = 0;
		eventData.Use();
	}

	public void OnDrag(PointerEventData eventData) {
		LineRenderer.positionCount++;
		Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
		pos.z = 10;
		LineRenderer.SetPosition(LineRenderer.positionCount-1, pos);
		
		eventData.Use();

	}

	public void OnPointerDown(PointerEventData eventData) {
	}

	public void OnPointerUp(PointerEventData eventData) {
	}
}
