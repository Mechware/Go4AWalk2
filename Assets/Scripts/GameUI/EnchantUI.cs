using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.UI;

public class EnchantUI : MonoBehaviour {

    public IconWithTextController Weapon;
    public IconWithTextController Enchanter;
    public IconWithTextController Result;

    public ItemViewer ItemViewer;

    public Button EnchantButton;

    public IntVariable NumberOfEnchantedWeapons;

    // Use this for initialization
    void Start () {
		Weapon.SetDataInstance(null, 0, "Put weapon to enchant here.", WeaponViewerClicked, null, true);
		Enchanter.SetDataInstance(null, 0, "Put item to enchant weapon with here.", EnchantViewerClicked, null, true);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        EnchantButton.interactable = false;
    }

    void WeaponViewerClicked() {
        ItemViewer.ShowItems("Enchantable Weapons", Inventory.Instance.Where(w => w is WeaponInstance instance && !instance.IsEnchanted), WeaponClicked, false);
        
        if(!Player.Instance.Weapon.IsEnchanted)
            ItemViewer.Add(Player.Instance.Weapon, 0, WeaponClicked);
    }

    void WeaponClicked(ItemInstance o ) {
        WeaponInstance w = (WeaponInstance) o;
        EnchantButton.interactable = Enchanter.Item.CurrentItem != null;
        Weapon.SetDataInstance(w, 1, $"{w.GetName(true, true)}\nDAM: {w.GetDamage()}", WeaponViewerClicked);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        ItemViewer.Close();
    }

    void EnchantViewerClicked() {
        ItemViewer.ShowItems("Enchantments", Inventory.Instance, EnchantClicked, false);
    }

    void EnchantClicked(ItemInstance o) {
        EnchanterInstance e = (EnchanterInstance) o;
        EnchantButton.interactable = Weapon.Item.CurrentItem != null;
        Enchanter.SetDataInstance(e, 1, $"{e.GetName()}\n{e.GetDescription()}", EnchantViewerClicked);
		Result.SetDataConfig(null, 0, "Result of enchantment goes here.", () => {});
        ItemViewer.Close();
    }

    public WeaponUI WeaponUI;

    public void Enchant() {
        WeaponInstance w = Weapon.Item.CurrentItem as WeaponInstance;
        EnchanterInstance e = Enchanter.Item.CurrentItem as EnchanterInstance;

        if (w == null || e == null) return;

        NumberOfEnchantedWeapons.Value++;

        Inventory.Instance.Remove(e);
        w.Enchant(e);

        Start();

        Result.SetDataInstance(w, 1, "Enchant Damage: " + w.GetEnchantDamage(), () => { WeaponUI.SetWeaponWithDefaults(w); });
        EquipItemProcessor.Instance.ProcessItem(w, null);
    }
}
