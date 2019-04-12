using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
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
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI DamageText;

    private Action<InventoryItemDisplay> OnClick;
    private Action<InventoryItemDisplay> OnHold;


	public void SetData( Item item, int amount, Action<InventoryItemDisplay> onclick = null, Action<InventoryItemDisplay> onhold = null) {

        LevelText.gameObject.SetActive(false);
        DamageText.gameObject.SetActive(false);

        Item = item;
        if(item == null) {
            ItemSprite.sprite = null;
            AmountText.gameObject.SetActive(false);
        } else {
            if(item.Rarity == Rarity.Common) {
                Background.color = new Color(255f, 255f, 255f); //white
            }
            else if (item.Rarity == Rarity.Uncommon) {
                Background.color = new Color(0f, 255f, 0f); //green
            } 
            else if (item.Rarity == Rarity.Rare) {
                Background.color = new Color(0f, 0f, 255f); //blue
            } 
            else if (item.Rarity == Rarity.VeryRare) {
                Background.color = new Color(128f, 0f, 128f); //purp
            } 
            else if (item.Rarity == Rarity.Legendary) {
                Background.color = new Color(255f, 215f, 0f); //gold
            } 
            else if (item.Rarity == Rarity. Mythical) {
                Background.color = new Color(0f, 0f, 0f); //black
            }

            ItemSprite.sprite = item.Image;
            AmountText.text = "x" +amount.ToString();
            AmountText.gameObject.SetActive(amount > 1);
            if(item is Weapon) {
                Weapon w = (Weapon)item;
                LevelText.text = " " + w.Mastery.ToString() + "\n<size=50%>" + "LVL " + w.Level.ToString();
                DamageText.text = "<size=50%> DAM <size=100%>" + w.ActualDamage.ToString();
                LevelText.gameObject.SetActive(true);
                DamageText.gameObject.SetActive(true);
            }
            if (item is Armor) {
                Armor a = (Armor)item;
                //LevelText.text = " " + 3/*a.Mastery.ToString()*/ + "\n<size=50%>" + "LVL " + 3/*a.Level.ToString()*/;
                DamageText.text = "<size=50%> ARM <size=100%>" + a.NoBlockModifier.ToString();
                //LevelText.gameObject.SetActive(true);
                DamageText.gameObject.SetActive(true);
            }
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
        StopAllCoroutines();
        holding = true;
        StartCoroutine(PointerHold());
    }

    public void OnPointerUp( PointerEventData eventData ) {
        if(holding) {
            OnClick?.Invoke(this);
        }
        holding = false;
        StopAllCoroutines();
    }

    IEnumerator PointerHold() {
        yield return new WaitForSeconds(1);
        if(holding) {
            holding = false;
            OnHold?.Invoke(this);
        }
    }
}
