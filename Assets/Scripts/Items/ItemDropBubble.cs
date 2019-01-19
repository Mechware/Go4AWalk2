using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemDropBubble : MonoBehaviour, IPointerClickHandler {

	public Item Item { get { return item; }}
	private Item item;

	public Image image;
	public Image background;

	[Tooltip("A rng with roll a number between 0 and 1 and then get the value from this anim curve")]
	public AnimationCurve XForce;
	[Tooltip("A rng with roll a number between 0 and 1 and then get the value from this anim curve")]
	public AnimationCurve YForce;

	public Action<ItemDropBubble> ItemClicked;

	private Rigidbody2D rb;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetData(Item it, Action<ItemDropBubble> OnClick) {
		item = it;
		image.sprite = item.image;
		ItemClicked = OnClick;
		background.color = ConfigObject.GetColorFromRarity(it.rarity);
	}

	[ContextMenu("Shoot")]
	public void Shoot() {
		rb.AddForce(new Vector2(XForce.Evaluate(Random.value), YForce.Evaluate(Random.value)));
	}

	public void OnPointerClick(PointerEventData eventData) {
		ItemClicked.Invoke(this);
	}
}
