using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;

public class CustomTap : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public GameEvent clickEvent;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(name + " Game Object Clicked!");
        clickEvent.Raise();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        clickEvent.Raise();
    }
}
