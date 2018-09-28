﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;

namespace G4AW2.Data.Inventory
{
    [CreateAssetMenu(menuName = "Data/Inventory")]
    public class InventoryList : ScriptableObject
    {
        //public List<Item> weaponList;
        //public List<Item> hatList;
        //public List<Item> armorList;
        //public List<Item> bootsList;
        public List<Item> materialList;
        public List<Item> consumableList;
        //public List<Item> accessoryList;
        public List<Item> equipmentList; //just one list to make inventory management easier

        public void getItem(Item item)
        {
            switch (item.type)
            {
                case ItemType.Accessory:
                    //accessoryList.Add(item);
                    equipmentList.Add(item);
                    break;
                case ItemType.Hat:
                    //hatList.Add(item);
                    equipmentList.Add(item);
                    break;
                case ItemType.Boots:
                    //bootsList.Add(item);
                    equipmentList.Add(item);
                    break;
                case ItemType.Consumable:
                    consumableList.Add(item);
                    break;
                case ItemType.Material:
                    materialList.Add(item);
                    break;
                case ItemType.Torso:
                    //armorList.Add(item);
                    equipmentList.Add(item);
                    break;
                case ItemType.Weapon:
                    //weaponList.Add(item);
                    equipmentList.Add(item);
                    break;
            }
        }


    }

   
}