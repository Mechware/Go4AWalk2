using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G4AW2.Data.DropSystem;

namespace G4AW2.Data.Inventory
{
    public class ItemDisplay : MonoBehaviour
    {
        private Item itemData;

        public void SetData(Item data)
        {
            itemData = data;
            GetComponent<Image>().sprite = data.image;
        }

    }
}
