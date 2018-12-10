using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public Item Item;

	public Image Background;
	public Image ItemSprite;
	public TextMeshProUGUI AmountText;

	private static Color HighlightColor = Color.yellow;

	public void SetData( Item item, int amount ) {
		ItemSprite.sprite = item.image;
		AmountText.text = amount.ToString();

		AmountText.gameObject.SetActive(amount > 1);
	}

#if UNITY_EDITOR
	[ContextMenu("SetItem")]
	public void SetItem() {
		if (Item == null) {
			throw new Exception("There's no item to set to...");
		}
		SetData(Item, 1);
	}
#endif

	private static bool holdingItem = false;
	private static InventoryItemDisplay currentlyOver = null;

	public void OnPointerClick(PointerEventData eventData) {
	}

	public void OnBeginDrag( PointerEventData eventData ) {
		holdingItem = true;
		eventData.Use();
	}

	public void OnDrag(PointerEventData eventData) {
		eventData.Use();
	}

	public void OnEndDrag(PointerEventData eventData) {
		holdingItem = false;
		if (currentlyOver != null) {
			int index = currentlyOver.transform.GetSiblingIndex();
			transform.SetSiblingIndex(index);
			currentlyOver = null;
		}
		eventData.Use();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (holdingItem) {
			Background.color = HighlightColor;
			currentlyOver = this;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		Background.color = Color.white;
		if (currentlyOver == this) currentlyOver = null;
	}
}
