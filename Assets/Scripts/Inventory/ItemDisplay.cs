using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G4AW2.Data.DropSystem;

namespace G4AW2.Data.Inventory
{
    public class ItemDisplay : MonoBehaviour
    {

        private Item item;

        public void SetData(Item data)
        {

                item = data;
            if(item != null)
                GetComponent<Image>().sprite = item.image;
            

        }

        public void setImage(Sprite image)
        {
            GetComponent<Image>().sprite = image;
        }


        public Item getItem()
        {
            return item;
        }
    }
}
