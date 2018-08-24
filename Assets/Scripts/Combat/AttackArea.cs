using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackArea : Graphic, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

	public UnityEvent OnTap;
	public UnityEvent OnSwipeStart;
    public UnityEventVector3Array OnSwipe;
	public UnityEventVector3Array OnSwipingDistanceChange;

	public float SimplifyAmount = 20;
	public LineRenderer LineRenderer;

	protected override void Awake() {
		color = new Color(0, 0, 0, 0);
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (dragging) return;
		OnTap.Invoke();
        eventData.Use();
	}

	public void StopDragging() {
		if (!dragging) return;
		dragging = false;

		LineRenderer.Simplify(SimplifyAmount);
		Vector3[] points = new Vector3[LineRenderer.positionCount];
		LineRenderer.GetPositions(points);
		OnSwipe.Invoke(points);
		LineRenderer.positionCount = 0;
	}

	private bool dragging = false;
	public void OnBeginDrag(PointerEventData eventData) {
		if (dragging) return;

		dragging = true;
		OnSwipeStart.Invoke();
		LineRenderer.positionCount = 0;
		eventData.Use();
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (!dragging) return; 

		dragging = false;
		LineRenderer.Simplify(SimplifyAmount);
		Vector3[] points = new Vector3[LineRenderer.positionCount];
		LineRenderer.GetPositions(points);
	    OnSwipe.Invoke(points);
        LineRenderer.positionCount = 0;
		eventData.Use();
	}

	Vector3[] line = new Vector3[2];
	public void OnDrag(PointerEventData eventData) {
		if (!dragging) return;

		LineRenderer.positionCount++;
		Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
		pos.z = 10;

		LineRenderer.SetPosition(LineRenderer.positionCount-1, pos);
		if (LineRenderer.positionCount > 1) {
			line[0] = LineRenderer.GetPosition(LineRenderer.positionCount - 1);
			line[1] = LineRenderer.GetPosition(LineRenderer.positionCount - 2);
			OnSwipingDistanceChange.Invoke(line);

		}

		eventData.Use();
	}

	public void OnPointerDown(PointerEventData eventData) {
	}

	public void OnPointerUp(PointerEventData eventData) {
	}
}