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

	public ItemInstance CurrentItem { private set; get; }

	[SerializeField] private Image Background;
    [SerializeField] private Image ItemSprite;
    [SerializeField] private TextMeshProUGUI AmountText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI DamageText;

    private Action<InventoryItemDisplay> OnClick;

	public void SetDataInstance( ItemInstance obj, 
        int amount, 
        Action<InventoryItemDisplay> onclick = null, 
        Sprite spriteOverride = null,
        bool showText = true) {

        CurrentItem = obj;

        ItemConfig item = obj.Config;
        
        if (obj is WeaponInstance w) {
            DamageText.text = $"<size=50%>DAM</size>\n<color=#c42c36>{w.RawDamage}</color>";
            DamageText.gameObject.SetActive(true);
        } else if (obj is ArmorInstance a) {
            DamageText.text = $"<size=50%>ARM</size>\n<color=#13b2f2>{a.ArmValue}</color>";
            DamageText.gameObject.SetActive(true);
        } else if (obj is HeadgearInstance h) {
            DamageText.text = $"<size=50%>HP</size>\n<color=#7bcf5c>{h.ExtraHealth}</color>";
            DamageText.gameObject.SetActive(true);
        }

        SetDataConfig(obj.Config, amount, onclick, spriteOverride, showText);
	}
    
    public void SetDataConfig( ItemConfig obj, 
        int amount, 
        Action<InventoryItemDisplay> onclick = null, 
        Sprite spriteOverride = null,
        bool showText = true) {

        CurrentItem = null;
	    ItemSprite.color = Color.white;

        LevelText.gameObject.SetActive(false);
        DamageText.gameObject.SetActive(false);

        
        ItemConfig item = obj;
        
        if(item == null) {
            ItemSprite.color = Color.clear;
            ItemSprite.sprite = null;
            AmountText.gameObject.SetActive(false);
        } else {
            Background.color = RarityDefines.Instance.GetColorFromRarity(item.Rarity);

            ItemSprite.sprite = spriteOverride ?? item.Image;
            AmountText.text = "x" +amount.ToString();
            AmountText.gameObject.SetActive(amount > 1);
        }

	    if (!showText) {
	        AmountText.gameObject.SetActive(false);
	        DamageText.gameObject.SetActive(false);
	        LevelText.gameObject.SetActive(false);
        }

        OnClick = onclick;
	}

    public void OnPointerClick(PointerEventData eventData) {
        OnClick?.Invoke(this);
    }
}
