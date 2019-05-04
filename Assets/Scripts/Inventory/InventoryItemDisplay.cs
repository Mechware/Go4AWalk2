using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour, IPointerClickHandler {

	public Item Item;

	public Image Background;
	public Image ItemSprite;
	public TextMeshProUGUI AmountText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI DamageText;

    private Action<InventoryItemDisplay> OnClick;

	public void SetData( Item item, int amount, 
        Action<InventoryItemDisplay> onclick = null, 
        Sprite spriteOverride = null,
        bool showText = true) {

	    ItemSprite.color = Color.white;

        LevelText.gameObject.SetActive(false);
        DamageText.gameObject.SetActive(false);

        Item = item;
        if(item == null) {
            ItemSprite.color = Color.clear;
            ItemSprite.sprite = null;
            AmountText.gameObject.SetActive(false);
        } else {
            if(item.Rarity == Rarity.Common) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.Common);
            }
            else if (item.Rarity == Rarity.Uncommon) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.Uncommon);
            } 
            else if (item.Rarity == Rarity.Rare) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.Rare);
            } 
            else if (item.Rarity == Rarity.VeryRare) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.VeryRare);
            } 
            else if (item.Rarity == Rarity.Legendary) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.Legendary);
            } 
            else if (item.Rarity == Rarity. Mythical) {
                Background.color = ConfigObject.GetColorFromRarity(Rarity.Mythical);
            }

            ItemSprite.sprite = spriteOverride ?? item.Image;
            AmountText.text = "x" +amount.ToString();
            AmountText.gameObject.SetActive(amount > 1);
            if(item is Weapon) {
                Weapon w = (Weapon)item;
                LevelText.text = " " + w.Mastery + "\n<size=50%>" + "LVL " + w.Level;
                DamageText.text = "<size=50%> DAM <size=100%>" + w.RawDamage;
                LevelText.gameObject.SetActive(true);
                DamageText.gameObject.SetActive(true);
            }
            if (item is Armor) {
                Armor a = (Armor)item;
                //LevelText.text = " " + 3/*a.Mastery.ToString()*/ + "\n<size=50%>" + "LVL " + 3/*a.Level.ToString()*/;
                DamageText.text = "<size=50%> ARM <size=100%>" + a.ARMValue;
                //LevelText.gameObject.SetActive(true);
                DamageText.gameObject.SetActive(true);
            }
        }

	    if (!showText) {
	        AmountText.gameObject.SetActive(false);
	        DamageText.gameObject.SetActive(false);
	        LevelText.gameObject.SetActive(false);
        }

        OnClick = onclick;
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

    public void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(this);
    }
}
