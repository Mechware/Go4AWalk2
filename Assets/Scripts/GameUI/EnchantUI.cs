using CustomEvents;
using G4AW2.Managers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnchantUI : MonoBehaviour {

    public IconWithTextController Weapon;
    public IconWithTextController Enchanter;
    public IconWithTextController Result;


    public Button EnchantButton;

    [Obsolete("Move this to the stat tracker")] public IntVariable NumberOfEnchantedWeapons;

    [SerializeField] private ItemManager _items;
    [SerializeField] private PlayerManager _player;
    
    [SerializeField] private EquipItemProcessor _itemUI;
    [SerializeField] private WeaponUI _weaponUI;
    [SerializeField] private ItemViewer _itemViewer;

    // Use this for initialization
    void Start () {
		Weapon.SetDataInstance(null, 0, "Put weapon to enchant here.", WeaponViewerClicked, null, true);
		Enchanter.SetDataInstance(null, 0, "Put item to enchant weapon with here.", EnchantViewerClicked, null, true);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        EnchantButton.interactable = false;
    }

    void WeaponViewerClicked() {
        _itemViewer.ShowItems("Enchantable Weapons", _items.Where(w => w is WeaponInstance instance && !instance.IsEnchanted), WeaponClicked, false);
        
        if(!_player.Weapon.IsEnchanted)
            _itemViewer.Add(_player.Weapon, 0, WeaponClicked);
    }

    void WeaponClicked(ItemInstance o ) {
        WeaponInstance w = (WeaponInstance) o;
        EnchantButton.interactable = Enchanter.Item.CurrentItem != null;
        Weapon.SetDataInstance(w, 1, $"{w.GetName(true, true)}\nDAM: {w.GetDamage()}", WeaponViewerClicked);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        _itemViewer.Close();
    }

    void EnchantViewerClicked() {
        _itemViewer.ShowItems("Enchantments", _items, EnchantClicked, false);
    }

    void EnchantClicked(ItemInstance o) {
        EnchanterInstance e = (EnchanterInstance) o;
        EnchantButton.interactable = Weapon.Item.CurrentItem != null;
        Enchanter.SetDataInstance(e, 1, $"{e.GetName()}\n{e.GetDescription()}", EnchantViewerClicked);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        _itemViewer.Close();
    }


    public void Enchant() {
        WeaponInstance w = Weapon.Item.CurrentItem as WeaponInstance;
        EnchanterInstance e = Enchanter.Item.CurrentItem as EnchanterInstance;

        if (w == null || e == null) return;

        NumberOfEnchantedWeapons.Value++;

        _items.Remove(e);
        w.Enchant(e);

        Start();

        Result.SetDataInstance(w, 1, "Enchant Damage: " + w.GetEnchantDamage(), () => { _weaponUI.SetWeaponWithDefaults(w); });
        _itemUI.ProcessItem(w, null);
    }
}
