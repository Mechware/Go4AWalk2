using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ChangeItem : MonoBehaviour {

    public InventoryItemDisplay IID;

    public WeaponReference Weapon;
    public ArmorReference Armor;
    public HeadgearReference Head;

    public ItemViewer Viewer;

    public enum ItemType { Head, Armour, Weapon}

    public ItemType ItType;

    // Use this for initialization
    public void Awake () {
        if (ItType == ItemType.Weapon) {
            Weapon.Variable.OnChange.AddListener((it) => IID.SetData(it, 1, Onclick, Onhold));
            IID.SetData(Weapon, 1, Onclick, Onhold);
        }
        if (ItType == ItemType.Armour) {
	        Armor.Variable.OnChange.AddListener((it) => IID.SetData(it, 1, Onclick, Onhold));
            IID.SetData(Armor, 1, Onclick, Onhold);
        }
        if (ItType == ItemType.Head) {
            Head.Variable.OnChange.AddListener((it) => IID.SetData(it, 1, Onclick, Onhold));
            IID.SetData(Head, 1, Onclick, Onhold);
        }
    }

    private void Onhold(InventoryItemDisplay inventoryItemDisplay) {
        throw new NotImplementedException();
    }

    private void Onclick(InventoryItemDisplay inventoryItemDisplay) {
        if (ItType == ItemType.Armour)
            Viewer.ShowItemsFromInventory<Armor>(false, it => Armor.Value = (Armor)it);
        else if (ItType == ItemType.Head)
            Viewer.ShowItemsFromInventory<Headgear>(false , it => Head.Value = (Headgear)it);
        else if (ItType == ItemType.Weapon)
            Viewer.ShowItemsFromInventory<Weapon>(false, it => Weapon.Value = (Weapon)it);

    }

    // Update is called once per frame
	void Update () {
		
	}
}
