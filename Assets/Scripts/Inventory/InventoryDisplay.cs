﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.UI;




namespace G4AW2.Data.Inventory
{
    public enum Type
    {
        Equipment = 0,
        Consumable = 1,
        Material = 2
    }

    public class InventoryDisplay : MonoBehaviour
    {
        public Camera cam;
        public RectTransform inventoryScreen;
        public GameObject itemPrefab;
        public InventoryList inventory;
        public Type itemType;
        private Vector2 startLocation, addRight, addDown; //possible to make addRight and addDown automatically made based on columns and rows, but im too lazy. Did it its fine.
        private RectTransform size;
        private Vector2 location;

        public float itemWidth;
        private float scaleFactor;

        public int columns, rows;

        private List<GameObject> itemList = new List<GameObject>();
        



        void Start()
        {

            scaleFactor = itemPrefab.GetComponent<RectTransform>().localScale.x;
            itemWidth = itemWidth*scaleFactor;
            size = GetComponent<RectTransform>();
            float width = size.rect.width;
            float height = size.rect.height;

            width = (width - itemWidth*columns)/(columns+1);
            height = (height - 30 - itemWidth*rows)/(rows+1);

            startLocation = new Vector2(-50+width+itemWidth/2, 35);
            addRight = new Vector2(width+itemWidth, 0);
            addDown = new Vector2(0, -height - itemWidth);

            ResetItems();

            
        }

        private void ResetItems()
        {
            itemList.ForEach(kvp => { Destroy(kvp.gameObject); });
            itemList.Clear();
            print("Cleared List");
            switch (itemType)
            {
                case Type.Equipment:
                    /*print("I want to make an equipment list");
                    for (int i = 0 ; i < inventory.weaponList.Count ; i++)
                    {
                        print("checking weapon slot " + i);
                        DisplayItem(inventory.weaponList[i]);
                    }
                    for (int i = 0 ; i < inventory.hatList.Count ; i++)
                    {
                        print("checking hat slot " + i);
                        DisplayItem(inventory.hatList[i]);
                    }
                    for (int i = 0 ; i < inventory.armorList.Count ; i++)
                    {
                        print("checking armor slot " + i);
                        DisplayItem(inventory.armorList[i]);
                    }
                    for (int i = 0 ; i < inventory.bootsList.Count ; i++)
                    {
                        print("checking boot slot " + i);
                        DisplayItem(inventory.bootsList[i]);
                    }
                    for (int i = 0 ; i < inventory.accessoryList.Count ; i++)
                    {
                        print("checking accessory slot " + i);
                        DisplayItem(inventory.accessoryList[i]);
                    }
                    print("Created Equipment List, its got " + itemList.Count + " items");*/

                    for (int i = 0 ; i < inventory.equipmentList.Count ; i++)
                    {
                        print("checking accessory slot " + i);
                        DisplayItem(inventory.equipmentList[i]);
                    }

                    break;
                case Type.Consumable:
                    for (int i = 0 ; i < inventory.consumableList.Count ; i++)
                    {
                        DisplayItem(inventory.consumableList[i]);
                    }
                    break;
                case Type.Material:
                    for (int i = 0 ; i < inventory.materialList.Count ; i++)
                    {
                        DisplayItem(inventory.materialList[i]);
                    }
                    break;
            }

            for (int i = 0 ; i < itemList.Count ; i++)
            {

                location.x = startLocation.x + addRight.x * (i - Mathf.Floor(i/columns) * columns);
                location.y = startLocation.y + addDown.y * Mathf.Floor(i/columns);

                //itemList[i].transform.localPosition = location;
                itemList[i].GetComponent<RectTransform>().anchoredPosition=location;
               
            }
        }

        void DisplayItem(Item item)
        {
            GameObject display = Instantiate(itemPrefab,transform);
            display.GetComponentInChildren<ItemDisplay>().SetData(item);
            display.GetComponent<DragItem>().cam = cam;
            display.GetComponent<DragItem>().inventory = inventoryScreen;
            itemList.Add(display);
        }



    }
}
