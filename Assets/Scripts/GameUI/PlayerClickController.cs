using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    public MyButton PlayerHeadgear;
    public MyButton PlayerArmor;
    public MyButton PlayerWeapon;

    public HeadgearReference HeadgearReference;
    public ArmorReference ArmorReference;
    public WeaponReference WeaponReference;

    public static PlayerClickController Instance;
    
    void Awake() {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHeadgear.onClick.AddListener(() => {
            ItemViewer.Instance.ShowItemsFromInventory<Headgear>("Equip Headgear", false, false, it => {
                PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", it.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                    () => {
                        if(HeadgearReference.Value != null)
                            DataManager.Instance.Inventory.Add(HeadgearReference.Value);
                        HeadgearReference.Value = it;
                        DataManager.Instance.Inventory.Remove(it);
                        ItemViewer.Instance.Close();
                    },
                    () => {
                        it.SetTrash(!it.IsTrash());
                        ItemViewer.Instance.Close();
                    },
                    () => { }
                });
            }); 
        });  
        
        PlayerArmor.onClick.AddListener(() => {
            ItemViewer.Instance.ShowItemsFromInventory<Armor>("Equip Armor", false, false, it => {
                PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", it.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                    () => {
                        if(ArmorReference.Value != null)
                            DataManager.Instance.Inventory.Add(ArmorReference.Value);
                        ArmorReference.Value = it;
                        DataManager.Instance.Inventory.Remove(it);
                        ItemViewer.Instance.Close();
                    },
                    () => {
                        it.SetTrash(!it.IsTrash());
                        ItemViewer.Instance.Close();
                    },
                    () => { }
                });
            }); 
        });

        PlayerWeapon.onClick.AddListener(() => {
            ItemViewer.Instance.ShowItemsFromInventory<Weapon>("Equip Weapon", false, false, it => {

                WeaponUI.Instance.SetWeapon(it, new [] {
                    new WeaponUI.ButtonAction() {Title = "Equip", OnClick = () => {
                        DataManager.Instance.Inventory.Add(WeaponReference.Value);
                        WeaponReference.Value = it;
                        DataManager.Instance.Inventory.Remove(it);
                        ItemViewer.Instance.Close();
                    }},
                    new WeaponUI.ButtonAction() {Title = it.IsTrash() ? "Untrash" : "Trash", OnClick = () => {
                        it.SetTrash(!it.IsTrash());
                        ItemViewer.Instance.Close();
                    }},
                    new WeaponUI.ButtonAction() {Title = "Close", OnClick = () => {}}
                });
            });
        });
    }

    public void SetEnabled(bool enabled) {
        PlayerWeapon.enabled = enabled;
        PlayerArmor.enabled = enabled;
        PlayerHeadgear.enabled = enabled;
    }
    
}
