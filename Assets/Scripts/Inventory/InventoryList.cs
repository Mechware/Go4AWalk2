using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using CustomEvents;

namespace G4AW2.Data.Inventory
{
    [CreateAssetMenu(menuName = "Data/Inventory")]
    public class InventoryList : ScriptableObject
    {
        //public List<Item> weaponList;
        //public List<Item> hatList;
        //public List<Item> armorList;
        //public List<Item> bootsList;
       // public GameObject itemPrefab;
        //public Camera cam;
       // public RectTransform inventoryScreen, playerReference;
        public RuntimeSetItem materialList;
        public RuntimeSetItem consumableList;
        //public List<Item> accessoryList;
        public RuntimeSetItem equipmentList; //just one list to make inventory management easier

        public GameEventItem itemAdded,itemRemoved;



		public void addItem(Item item)
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
     
            itemAdded.Raise(item);
        }

        public void insertItem(Item item, int index)
        {
            switch (item.type)
            {
                case ItemType.Accessory:
                    //accessoryList.Add(item);
                    equipmentList.Insert(index,item);
                    break;
                case ItemType.Hat:
                    //hatList.Add(item);
                    equipmentList.Insert(index, item);
                    break;
                case ItemType.Boots:
                    //bootsList.Add(item);
                    equipmentList.Insert(index, item);
                    break;
                case ItemType.Consumable:
                    consumableList.Insert(index, item);
                    break;
                case ItemType.Material:
                    materialList.Insert(index, item);
                    break;
                case ItemType.Torso:
                    //armorList.Add(item);
                    equipmentList.Insert(index, item);
                    break;
                case ItemType.Weapon:
                    //weaponList.Add(item);
                    equipmentList.Insert(index, item);
                    break;
            }
        }

       /* public void addItem(Item item, int itemIndex)
        {
            GameObject display = Instantiate(itemPrefab);
            display.GetComponentInChildren<ItemDisplay>().SetData(item);
            display.GetComponent<DragItem>().cam = cam;
            display.GetComponent<DragItem>().inventory = inventoryScreen;
            display.GetComponent<DragItem>().playerReference=playerReference;
            //display.GetComponent<DragItem>().assignGrid(startLocation, add);
            if (itemIndex == -1) addItemToList(display);
            else insertItem(display, itemIndex);
        }*/

        public void removeItem(Item item)
        {
            switch (item.type)
            {
                case ItemType.Accessory:
                    //accessoryList.Add(item);
                    equipmentList.Remove(item);
                    break;
                case ItemType.Hat:
                    //hatList.Add(item);
                    equipmentList.Remove(item);
                    break;
                case ItemType.Boots:
                    //bootsList.Add(item);
                    equipmentList.Remove(item);
                    break;
                case ItemType.Consumable:
                    consumableList.Remove(item);
                    break;
                case ItemType.Material:
                    materialList.Remove(item);
                    break;
                case ItemType.Torso:
                    //armorList.Add(item);
                    equipmentList.Remove(item);
                    break;
                case ItemType.Weapon:
                    //weaponList.Add(item);
                    equipmentList.Remove(item);
                    break;
            }
        }

        public void removeItem(Item item, int index)
        {
            switch (item.type)
            {
                case ItemType.Accessory:
                    //accessoryList.Add(item);
                    equipmentList.RemoveAt(index);
                    break;
                case ItemType.Hat:
                    //hatList.Add(item);
                    equipmentList.RemoveAt(index);
                    break;
                case ItemType.Boots:
                    //bootsList.Add(item);
                    equipmentList.RemoveAt(index);
                    break;
                case ItemType.Consumable:
                    consumableList.RemoveAt(index);
                    break;
                case ItemType.Material:
                    materialList.RemoveAt(index);
                    break;
                case ItemType.Torso:
                    //armorList.Add(item);
                    equipmentList.RemoveAt(index);
                    break;
                case ItemType.Weapon:
                    //weaponList.Add(item);
                    equipmentList.RemoveAt(index);
                    break;
            }
	        itemRemoved.Raise(item);
		}


    }

   
}
