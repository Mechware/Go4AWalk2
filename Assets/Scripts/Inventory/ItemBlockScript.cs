using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.UI;

namespace G4AW2.Data.Inventory
{

    public class ItemBlockScript : MonoBehaviour
    {

        public Item item;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void applyItem(Item item)
        {
            this.item = item;
            GetComponent<Image>().sprite = item.image;
        }


    }
}
