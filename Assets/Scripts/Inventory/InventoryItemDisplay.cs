using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Item Item;

	public Image Background;
	public Image ItemSprite;
	public TextMeshProUGUI AmountText;

    private Action<InventoryItemDisplay> OnClick;
    private Action<InventoryItemDisplay> OnHold;

	public void SetData( Item item, int amount, Action<InventoryItemDisplay> onclick = null, Action<InventoryItemDisplay> onhold = null) {
        Item = item;
        if(item == null) {
            ItemSprite.sprite = null;
            AmountText.gameObject.SetActive(false);
        } else {
            ItemSprite.sprite = item.image;
            AmountText.text = amount.ToString();
            AmountText.gameObject.SetActive(amount > 1);
        }
		
        OnClick = onclick;
        OnHold = onhold;
	}

#if UNITY_EDITOR
    /// <summary>
    /// For testing ONLY.
    /// </summary>
	[ContextMenu("SetItem")]
	public void SetItem() {
		if (Item == null) {
			throw new Exception("There's no item to set to...");
		}
		SetData(Item, 1, (it) => { });
	}
#endif

    bool holding = false;
    public void OnPointerDown( PointerEventData eventData ) {
        print("Pointer down");
        StopAllCoroutines();
        holding = true;
        StartCoroutine(PointerHold());
    }

    public void OnPointerUp( PointerEventData eventData ) {
        print("Pointer up");
        if(holding) {
            print("Clicked item");
            OnClick?.Invoke(this);
        }
        holding = false;
        StopAllCoroutines();
    }

    IEnumerator PointerHold() {
        yield return new WaitForSeconds(1);
        if(holding) {
            holding = false;
            print("Hold");
            OnHold?.Invoke(this);
        }
    }
}
