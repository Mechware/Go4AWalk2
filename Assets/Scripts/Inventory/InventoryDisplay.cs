using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.UI;
using G4AW2.Utils;

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
        public RectTransform playerReference;
        public GameObject itemPrefab;
        public InventoryList inventory;
        public Type itemType;
        private Vector2 startLocation, add;
        private RectTransform size;
        private Vector2 location;
        private Vector2 startCenterLocation;
        public GameObject[] buttons; //depending on if we change the buttons, this will change.

        public float itemWidth;
        private float scaleFactor;

        public int columns, rows;

        private List<GameObject> itemList = new List<GameObject>();

        private float height;


        void Start()
        {
            startCenterLocation = GetComponent<RectTransform>().anchoredPosition; // get starting location
            scaleFactor = itemPrefab.GetComponent<RectTransform>().localScale.x; //scale of items in scene
            itemWidth = itemWidth*scaleFactor; //make sure the width is the proper scale
            size = inventoryScreen; 
            float width = size.rect.width; // assign height and width of inventoryScreen rectTransform. height is global because fuck you
            height = size.rect.height;

            width = (width - 15 - itemWidth*columns)/(columns+1); //fun math to find the spacing required between items
            height = (height - 25 - itemWidth*rows)/(rows+1); // the 15 and the 25 are tweaks

            print(size.rect.height);
            startLocation = new Vector2(-50+width+itemWidth/2, size.rect.height/2-30); // make the startLocation of items the top left
            add.x = width+itemWidth; //this is how much we move the items in the display
            add.y = -height - itemWidth;

            

            ResetItems(); // reset it just because something might have changed

            
        }


        public void resetTop()
        {
            GetComponent<RectTransform>().anchoredPosition = inventoryScreen.rect.center-new Vector2(0, GetComponent<RectTransform>().rect.height/2)+new Vector2(0, inventoryScreen.rect.height/2+10); // the inventory will display the top when opened. If I didn't do this it does weird stuff
            GetComponent<DragObject>().slider.GetComponent<RectTransform>().localPosition = GetComponent<DragObject>().sliderMaxBounds;
        }


        private void ResetItems()
        {
            itemList.ForEach(kvp => { Destroy(kvp.gameObject); }); //**KILL_THEM_ALL**??//##
            itemList.Clear();
            print("Cleared List");
            switch (itemType)
            {
                case Type.Equipment: // display items in the inventory if it matches

                    for (int i = 0 ; i < inventory.equipmentList.Count ; i++)
                    {
                        DisplayItem(inventory.equipmentList[i],-1);
                    }

                    break;
                case Type.Consumable:
                    for (int i = 0 ; i < inventory.consumableList.Count ; i++)
                    {
                        DisplayItem(inventory.consumableList[i],-1);
                    }
                    break;
                case Type.Material:
                    for (int i = 0 ; i < inventory.materialList.Count ; i++)
                    {
                        DisplayItem(inventory.materialList[i],-1);
                    }
                    break;
            }

            ChangePositions(itemList,0); // make sure the list looks all nice and proper

        }

        private void ChangePositions(List<GameObject> list, int startingIndex)
        {
            //changes position of items if you move them around
            RectTransform rt = GetComponent<RectTransform>();

            bool changedSize = false;

            int changedRows = (int)Mathf.Floor(itemList.Count/columns)+1; // how many rows should we have now?
            if (changedRows < rows) changedRows = rows;
            rt.sizeDelta = new Vector2(rt.rect.width, changedRows*(itemWidth+height)+30); //change the height based on the rows please
            startLocation.y = rt.rect.yMax-30; // now you gotta change the start
            GetComponent<DragObject>().MaxBounds = new Vector2(0, inventoryScreen.rect.center.y+GetComponent<RectTransform>().rect.height/2-inventoryScreen.rect.height/2); //make drag screen bounds all nice and friendly 
            GetComponent<DragObject>().MinBounds = new Vector2(0, inventoryScreen.rect.center.y-GetComponent<RectTransform>().rect.height/2+inventoryScreen.rect.height/2+10);// that plus 10 is because im lazy

            if(itemList.Count - Mathf.Floor((itemList.Count)/columns) * columns == 0) // some weird indexing issue happening here. seems to be extending the rect before I want it to. not a big deal though
                changedSize = true; // just so we don't rebuild the whole list every time

            print(changedSize);



            if (changedSize) startingIndex = 0;
            for (int i = startingIndex ; i < list.Count ; i++)
            {
                location.x = startLocation.x + add.x * (i - Mathf.Floor(i/columns) * columns); // returns 0,1,2,3 ... for x position of item
                location.y = startLocation.y + add.y * Mathf.Floor(i/columns); // "" for y position of item

                list[i].GetComponent<RectTransform>().anchoredPosition=location; // move the item there~


            }


        }


        void DisplayItem(Item item, int itemIndex)
        {
            GameObject display = Instantiate(itemPrefab,transform); //instantiate as parent
            display.GetComponentInChildren<ItemDisplay>().SetData(item); //set the item
            display.GetComponent<DragItem>().cam = cam; // set the camera
            display.GetComponent<DragItem>().inventory = inventoryScreen; //set the inventory reference
            display.GetComponent<DragItem>().playerReference=playerReference; // set the player reference
            display.GetComponent<DragItem>().dragScreen = GetComponent<RectTransform>(); //set the reference for this rectTransform
            if (itemIndex == -1) itemList.Add(display); //because I didn't want to make an overload for this
            else itemList.Insert(itemIndex,display);
            if (GetComponentInChildren<GraphicRaycaster>().enabled == false) disableRaycasts(null);
 
        }

        public Vector3[] getGrid() // gets the start location and the amount we move the items. 
        {
            Vector3[] array = new Vector3[2];
            array[0] = startLocation;
            array[1] = add;
            return array;
        }

        void DisplayItem(GameObject item, int itemIndex, int itemCurrentIndex) // I made an overload anyway. This is if the item moves to a spot infront of where it was
        {
            itemList.Insert(itemIndex, item);
            
            if (itemCurrentIndex < itemIndex) itemIndex = itemCurrentIndex;
            ChangePositions(itemList, itemIndex);

        }


        public void addItem(Item item)
        {
            //Item item = inventory.lastItemAdded;
            if(itemType == Type.Equipment && (item.type == ItemType.Accessory || item.type == ItemType.Boots || item.type == ItemType.Weapon || item.type == ItemType.Hat || item.type == ItemType.Torso))       
                DisplayItem(item, -1);

            if(itemType == Type.Consumable && item.type == ItemType.Consumable)
                DisplayItem(item, -1);

            if (itemType == Type.Material && item.type == ItemType.Material)
                DisplayItem(item, -1);

            ChangePositions(itemList, itemList.Count-1);

            print("item added");
            //inventory.lastItemAdded;
        }


        public void removeItem(GameObject item, List<GameObject> list)
        {
            int index = list.FindIndex(itemObject => itemObject == item);
            inventory.removeItem(list[index].GetComponentInChildren<ItemDisplay>().getItem(),index);
            
            list.Remove(item);
            Destroy(item);
            ChangePositions(list, index);
        }

        public void removeItem(GameObject item)
        {
            int index = itemList.FindIndex(itemObject => itemObject == item);
            inventory.removeItem(itemList[index].GetComponentInChildren<ItemDisplay>().getItem(),index);
            
            itemList.Remove(item);
            Destroy(item);
            ChangePositions(itemList, index);
        }


        //for testing only
        public void removeItem()
        {
            GameObject obj = itemList[itemList.Count-1];
            itemList.RemoveAt(itemList.Count-1);
            Destroy(obj);
            ChangePositions(itemList, 0);
        }
        //End for testing only


        public void moveItem(GameObject item, int index)
        {
            if (index <= -1) return;
            if (index >= itemList.Count) index = itemList.Count-1;

            int currentIndex = itemList.FindIndex(itemObject => itemObject == item);

            itemList.Remove(item);
            inventory.removeItem(item.GetComponentInChildren<ItemDisplay>().getItem(),currentIndex);
            inventory.insertItem(item.GetComponentInChildren<ItemDisplay>().getItem(), index);
            DisplayItem(item, index, currentIndex);
        }

        public void disableRaycasts(GameObject ignore)
        {
            Component[] rays = GetComponentsInChildren(typeof(GraphicRaycaster));
            foreach(GraphicRaycaster ray in rays)
                ray.enabled=false;
            if(ignore != null)
            {
                ignore.GetComponent<GraphicRaycaster>().enabled= true;
            }

        }
        public void disableRaycasts()
        {
            Component[] rays = GetComponentsInChildren(typeof(GraphicRaycaster));
            foreach (GraphicRaycaster ray in rays)
                ray.enabled=false;
            foreach (GameObject obj in buttons) obj.GetComponent<GraphicRaycaster>().enabled=false; //might change depends on buttons
        }

        public void enableRaycasts()
        {
            Component[] rays = GetComponentsInChildren(typeof(GraphicRaycaster));
            foreach (GraphicRaycaster ray in rays)
                ray.enabled=true;
            foreach (GameObject obj in buttons) obj.GetComponent<GraphicRaycaster>().enabled=true; //might change depends on buttons
        }



        public Vector3 startCenter()
        {
            return startCenterLocation;
        }





    }
}
