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

    public class EquipDisplay : MonoBehaviour, IPointerClickHandler // dont worry about the name of the script. It needs to get changed but it will need a few things to change and im lazy
    {


        //do this later with passing data through events, it'll work better probably.

        public GameObject equipInventory, currentEquip;

        public UnityEvent openEquipDisplay;


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
