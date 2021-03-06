using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickReceiver : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public UnityEvent MouseClick;

	public UnityEvent MouseDown;
	public UnityEvent MouseUp;
	
	public void OnPointerClick(PointerEventData eventData) {
		//Vector3 vec = Camera.main.ScreenToWorldPoint(eventData.position);
		//vec.z = 0;
		MouseClick?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData) {
		//Vector3 vec = Camera.main.ScreenToWorldPoint(eventData.position);
		//vec.z = 0;
		MouseDown?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData) {
		//Vector3 vec = Camera.main.ScreenToWorldPoint(eventData.position);
		//vec.z = 0;
		MouseUp?.Invoke();
	}
}
