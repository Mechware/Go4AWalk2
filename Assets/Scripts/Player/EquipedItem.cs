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
        public GameObject currentEquip, equipDisplay;
        public Sprite background;

        // public UnityEvent OnAwake;


        // Use this for initialization
        void Start()
        {
            setSprite();
            currentEquip.SetActive(false);
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
                SetItemStart(item);
        }


        public void SetItem(Item itemToSet)
        {
            if (itemToSet.type == type)
            {

                setAnimation(itemToSet);

                player.removeItem(type);
                equipDisplay.GetComponentInChildren<ItemDisplay>().SetData(item);
                player.setItem(itemToSet);
                setSprite();
            }

        }

        public void SetItemStart(Item itemToSet)
        {
            if (itemToSet.type == type)
            {

                setAnimation(itemToSet);

                //player.removeItem(type);
                equipDisplay.GetComponentInChildren<ItemDisplay>().SetData(item);
                player.setItem(itemToSet);
                setSprite();
            }

        }

        private void setAnimation(Item itemToSet)
        {
            item = itemToSet;

            AnimationClip clip = item.Walking;

            if (clip == null)
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

        private void setSprite()
        {
            if (player.returnItem(type) != null)
            {
               // equipDisplay.GetComponentInChildren<Image>().sprite = player.returnItem(type).image;
                equipDisplay.GetComponent<GraphicRaycaster>().enabled = true;
            } else
            {
                equipDisplay.GetComponentInChildren<ItemDisplay>().setImage(background);
                equipDisplay.GetComponent<GraphicRaycaster>().enabled = false;
            }
        }

        public void unEquip(Item itemToUnequip)
        {
            if (itemToUnequip == item && item != null)
            {
                item = null;
                player.removeItem(itemToUnequip.type);
                equipDisplay.GetComponentInChildren<ItemDisplay>().SetData(item);
                setSprite();
                GetComponent<Image>().enabled = false;

            }


        }
    }
}
