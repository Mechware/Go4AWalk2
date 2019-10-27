using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using Items;
using UnityEditor;
using UnityEngine;

public class EquipItemProcessor : MonoBehaviour {
    public static EquipItemProcessor Instance;

    public Inventory Inventory => DataManager.Instance.Inventory;
    public Player Player => DataManager.Instance.Player;
    
    void Awake() {
        Instance = this;
    }

    public bool ProcessItem(Item it, Action onDone) {

        if (it is Weapon w) {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", w.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(Player.Weapon.Value != null)
                        Inventory.Add(Player.Weapon.Value);
                    Player.Weapon.Value = w;
                    Inventory.Remove(w);
                    onDone?.Invoke();
                },
                () => {
                    w.SetTrash(!w.IsTrash());
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;
        } 
        
        if (it is Armor a) {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", a.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(Player.Armor.Value != null)
                        Inventory.Add(Player.Armor.Value);
                    Player.Armor.Value = a;
                    Inventory.Remove(a);
                    onDone?.Invoke();
                },
                () => {
                    a.SetTrash(!a.IsTrash());
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;

        } 
        
        if (it is Headgear h) {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", h.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(Player.Headgear.Value != null)
                        Inventory.Add(Player.Headgear.Value);
                    Player.Headgear.Value = h;
                    Inventory.Remove(h);
                    onDone?.Invoke();
                },
                () => {
                    h.SetTrash(!h.IsTrash());
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;

        } 

        if(it is Consumable c) {
            ConsumableManager.Instance.UseConsumable(c);
            Inventory.Remove(c);
        }
        
        onDone?.Invoke();
        return false;
    }
}
