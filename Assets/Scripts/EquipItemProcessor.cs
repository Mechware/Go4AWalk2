using G4AW2.Component.UI;
using G4AW2.Managers;
using System;
using UnityEngine;

[Obsolete("Move this into the game ui class. Renamed the functions, they aren't processing anything, they're letting the user decide what to do with equipment")]
public class EquipItemProcessor : MonoBehaviour {
    [Obsolete("Singleton")] public static EquipItemProcessor Instance;

    [SerializeField] private PlayerManager _player;
    [SerializeField] private ItemManager _items;
    [SerializeField] private PopUp _popUp;

    void Awake() {
        Instance = this;
    }

    public bool ProcessItem(object it, Action onDone) {

        if (it is WeaponInstance w) {
            _popUp.SetPopUpNew($"{w.GetName(true, true)}\n{w.GetDescription()}", new string[] {"Equip", w.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(_player.Weapon != null)
                        _items.Add(_player.Weapon);
                    _player.Weapon = w;
                    _items.Remove(w);
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
            _popUp.SetPopUpNew($"{a.GetName()}\n{a.GetDescription()}", new string[] {"Equip", a.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(_player.Armor != null)
                        _items.Add(_player.Armor);
                    _player.Armor = a;
                    _items.Remove(a);
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
            _popUp.SetPopUpNew($"{h.GetName()}\n{h.GetDescription()}", new string[] {"Equip", h.SaveData.MarkedAsTrash ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(_player.Headgear != null)
                        _items.Add(_player.Headgear);
                    _player.Headgear = h;
                    _items.Remove(h);
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
                    ItemManager.Remove(c);
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
