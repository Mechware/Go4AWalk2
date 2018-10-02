using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Combat;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace G4AW2.Data.Inventory
{

    public class EquipDisplay : MonoBehaviour, IPointerClickHandler
    {


        //do this later with passing data through events, it'll work better probably.

        public GameObject hatDisplay, weaponDisplay, armorDisplay, bootsDisplay, equipInventory, currentEquip;

        public Player player;
        public InventoryList inventory;
        public UnityEvent openEquipDisplay;


        /*void Start()
        {
            currentEquip.SetActive(false);
        }


        public void EquipItem(GameObject itemObject)
        {
            Item item = itemObject.GetComponentInChildren<ItemDisplay>().getItem();
            unEquipItem(item);
            switch (item.type)
            {
                case (ItemType.Accessory):
                    if (player.accessory != null)
                    {
                        //still need to work on accessories & if we want them & what they'll affect. idk what to put here
                    }
                    break;
                case (ItemType.Weapon):
                    player.weapon = item;
                    player.Damage.Value = item.value;
                    equipInventory.GetComponent<InventoryDisplay>().removeItem(itemObject);
                    weaponDisplay.GetComponent<EquipedItem>().SetItem(item);

                    break;
                case (ItemType.Hat):
                    player.hat = item;
                    player.Armor.Value = player.Armor.Value-item.value;
                    equipInventory.GetComponent<InventoryDisplay>().removeItem(itemObject);
                    hatDisplay.GetComponent<EquipedItem>().SetItem(item);
                    break;
                case (ItemType.Boots):
                    player.boots = item;
                    player.Armor.Value = player.Armor.Value + item.value;
                    equipInventory.GetComponent<InventoryDisplay>().removeItem(itemObject);
                    bootsDisplay.GetComponent<EquipedItem>().SetItem(item);
                    break;
                case (ItemType.Torso):
                    player.armor = item;
                    player.Armor.Value = player.Armor.Value-item.value;
                    equipInventory.GetComponent<InventoryDisplay>().removeItem(itemObject);
                    armorDisplay.GetComponent<EquipedItem>().SetItem(item);
                    break;
                case (ItemType.Material):
                    break;
                case (ItemType.Consumable):
                    break;

            }

            currentEquip.GetComponent<CurrentEquip>().updateDisplay();

        }

        public void unEquipItem(Item item)
        {
            switch (item.type)
            {
                case (ItemType.Accessory):
                    if (player.accessory != null)
                    {
                        //still need to work on accessories & if we want them & what they'll affect. idk what to put here
                    }
                    break;
                case (ItemType.Weapon):
                    if (player.weapon != null)
                    {
                        player.Damage.Value = 1;//player.Damage.Value-player.weapon.value;
                        inventory.addItem(player.weapon);
                        player.weapon = null;
                    }

                    break;
                case (ItemType.Hat):
                    if (player.hat != null)
                    {
                        player.Armor.Value = player.Armor.Value-player.hat.value;
                        inventory.addItem(player.hat);
                        player.hat = null;
                    }

                    break;
                case (ItemType.Boots):
                    if (player.boots != null)
                    {
                        player.Armor.Value = player.Armor.Value-player.boots.value;
                        inventory.addItem(player.boots);
                        player.boots = null;
                    }


                    break;
                case (ItemType.Torso):
                    if (player.armor == null)
                    {
                        player.Armor.Value = player.Armor.Value-player.armor.value;
                        inventory.addItem(player.armor);
                        player.armor = null;
                    }

                    break;
                case (ItemType.Material):
                    break;
                case (ItemType.Consumable):
                    break;

            }

            currentEquip.GetComponent<CurrentEquip>().updateDisplay();
        }*/


        public void OnPointerClick(PointerEventData eventData)
        {
            if (equipInventory.activeSelf)
            {
                currentEquip.SetActive(true);
                openEquipDisplay.Invoke();
                
            }

        }
    }
}
