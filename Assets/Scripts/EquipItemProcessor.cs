using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEditor;
using UnityEngine;

public class EquipItemProcessor : MonoBehaviour {
    public static EquipItemProcessor Instance;

    public Inventory Inventory => DataManager.Instance.Inventory;
    public Player Player => DataManager.Instance.Player;
    
    void Awake() {
        Instance = this;
    }

    public void ProcessItem(Item it, Action onDone) {

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
        } else if (it is Armor a) {
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
        } else if (it is Headgear h) {
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
        } else if (it is InstrumentData i) {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", "Cancel"}, new Action[] {
                () => {
                    if(Player.Instrument.Value != null)
                        Inventory.Add(Player.Instrument.Value);
                    Player.Instrument.Value = i;
                    Inventory.Remove(i);
                    onDone?.Invoke();
                },
                () => {
                    onDone?.Invoke();
                }
            });
        }
        else {
            onDone?.Invoke();
        }
    }
}
