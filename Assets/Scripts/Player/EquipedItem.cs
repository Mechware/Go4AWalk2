using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.Events;
using UnityEngine.UI;

namespace G4AW2.Data.Inventory
{
    [RequireComponent(typeof(Animator))]
    public class EquipedItem : MonoBehaviour
    {

        public Item item;

       // public UnityEvent OnAwake;
        

        // Use this for initialization
        void Start()
        {


            GetComponent<Image>().enabled = false;
            if (item != null)
                SetItem(item);
        }


        public void SetItem(Item itemToSet)
        {
            item = itemToSet;

            AnimationClip clip = item.Walking;

            if(clip == null)
            {
                print("clip is null");
            }

            GetComponent<Image>().sprite = item.image;
            GetComponent<Image>().enabled = true;

            AnimatorOverrideController aoc = (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;

            aoc["PlayerWalking"] = clip;

            GetComponent<Animator>().SetTrigger("PlayerWalking");


        }
    }
}
