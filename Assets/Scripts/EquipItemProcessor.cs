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

    
    void Awake() {
        Instance = this;
    }

    public bool ProcessItem(object it, Action onDone) {

        Player p = Player.Instance;
        Inventory i = Inventory.Instance;
        
        if (it is WeaponInstance w) {
            PopUp.SetPopUp($"{w.GetName(true, true)}\n{w.GetDescription()}", new string[] {"Equip", w.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(p.Weapon != null)
                        i.Add(p.Weapon);
                    p.Weapon = w;
                    i.Remove(w);
                    onDone?.Invoke();
                },
                () => {
                    w.SaveData.MarkedAsTrash = !w.SaveData.MarkedAsTrash;
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;
        } 
        
        if (it is ArmorInstance a) {
            PopUp.SetPopUp($"{a.GetName()}\n{a.GetDescription()}", new string[] {"Equip", a.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(p.Armor != null)
                        i.Add(p.Armor);
                    p.Armor = a;
                    i.Remove(a);
                    onDone?.Invoke();
                },
                () => {
                    a.SaveData.MarkedAsTrash = !a.SaveData.MarkedAsTrash;
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;

        } 
        
        if (it is HeadgearInstance h) {
            PopUp.SetPopUp($"{h.GetName()}\n{h.GetDescription()}", new string[] {"Equip", h.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(p.Headgear != null)
                        i.Add(p.Headgear);
                    p.Headgear = h;
                    i.Remove(h);
                    onDone?.Invoke();
                },
                () => {
                    h.SaveData.MarkedAsTrash = !h.SaveData.MarkedAsTrash;
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;
        } 

        /*
        if(it is Consumable c) {
            PopUp.SetPopUp($"{c.GetName()}\n{it.GetDescription()}\nDuration:{c.Duration}", new string[] { "Use", "Close" }, new Action[] {
                () => {
                    if(ConsumableManager.Instance.UseConsumable(c)) {
                        ConsumableUi.Instance.Refresh();
                    }
                    Inventory.Remove(c);
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
            return true;
        }*/
        
        onDone?.Invoke();
        return false;
    }
}
