using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MyButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

	public enum ButtonType {
		SpriteSwap,
		Tint,
		None
	}

	public RectTransform RectTransform => (RectTransform) transform;
	
	public float MoveAmount = 2;
	
	public ButtonType InteractionType;

	[DrawIf("InteractionType", ButtonType.SpriteSwap)]
	public Sprite DefaultSprite;

	[DrawIf("InteractionType", ButtonType.SpriteSwap)]
	public Sprite ClickedSprite;

	[DrawIf("InteractionType", ButtonType.SpriteSwap)]
	public Sprite DisabledSprite;
	
	[DrawIf("InteractionType", ButtonType.Tint)]
	public Color DefaultTint;

	[DrawIf("InteractionType", ButtonType.Tint)]
	public Color ClickedTint;

	[DrawIf("InteractionType", ButtonType.Tint)]
	public Color DisabledTint;

	private enum ButtonState {
		Up, Down, Disabled
	}

	private ButtonState state = ButtonState.Up;

	private bool interactable => state != ButtonState.Disabled;

	public UnityEvent onClick;

	public Image image;

	void Awake() {
		image = GetComponent<Image>();
	}

	public void Enable() {
		if (interactable) return;
		
		state = ButtonState.Up;
		SetState();
	}

	public void Disable() {
		if (!interactable) return;
		
		state = ButtonState.Disabled;
		SetState();
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (!interactable) return;

		onClick.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (!interactable) return;
		state = ButtonState.Down;
		
		SetState();
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (!interactable) return;
		state = ButtonState.Up;
	
		SetState();
	}

	private Vector2 CurrentOffset = Vector2.zero;

	public void SetState() {

		RectTransform.anchoredPosition = RectTransform.anchoredPosition - CurrentOffset;
		CurrentOffset = Vector2.zero;

		if (state == ButtonState.Disabled) {
			if (InteractionType == ButtonType.SpriteSwap) {
				image.sprite = DisabledSprite;
			}
			else if (InteractionType == ButtonType.Tint) {
				image.color = DisabledTint;
			}
		} 
		else if (state == ButtonState.Down) {
			CurrentOffset = new Vector2(MoveAmount, -MoveAmount);

			if (InteractionType == ButtonType.SpriteSwap) {
				image.sprite = ClickedSprite;
			}
			else if (InteractionType == ButtonType.Tint) {
				image.color = ClickedTint;
			}
		}
		else if (state == ButtonState.Up) {
			if (InteractionType == ButtonType.SpriteSwap) {
				image.sprite = DefaultSprite;
			}
			else if (InteractionType == ButtonType.Tint) {
				image.color = DefaultTint;
			}
		}
		
		RectTransform.anchoredPosition = RectTransform.anchoredPosition + CurrentOffset;

	}
}
