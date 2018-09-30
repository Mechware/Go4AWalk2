using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using G4AW2.Combat;

namespace G4AW2.Data.Inventory
{
    [RequireComponent(typeof(Animator))]
    public class EquipedItem : MonoBehaviour
    {

        public Item item;
        public Player player;
        public ItemType type;

       // public UnityEvent OnAwake;
        

        // Use this for initialization
        void Start()
        {
            switch (type)
            {
                case (ItemType.Boots):
                    if (player.boots != null) item = player.boots;
                    break;
                case (ItemType.Hat):
                    if (player.hat != null) item = player.hat;
                    break;
                case (ItemType.Weapon):
                    if (player.weapon != null) item = player.weapon;
                    break;
                case (ItemType.Torso):
                    if (player.armor != null) item = player.armor;
                    break;
            }

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
            GetComponentInParent<Animator>().SetTrigger("PlayerWalking");


        }
    }
}
