using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;

public class ChangeItemWeapon : ChangeItemBase<Weapon, WeaponReference, WeaponVariable, UnityEventWeapon> {

    protected override void Onclick(InventoryItemDisplay inventoryItemDisplay) {

        Viewer.ShowItemsFromInventory<Weapon>(Title, false, ShowTrash, it => {

            WeaponUI.Instance.SetWeapon(it, new [] {
                new WeaponUI.ButtonAction() {Title = "Equip", OnClick = () => {
                    Inventory.Add(Item.Value);
                    Item.Value = it;
                    Inventory.Remove(it);
                    Viewer.Close();
                }},
                new WeaponUI.ButtonAction() {Title = it.IsTrash() ? "Untrash" : "Trash", OnClick = () => {
                    it.SetTrash(!it.IsTrash());
                    Viewer.Close();
                    Onclick(null);
                }},
                new WeaponUI.ButtonAction() {Title = "Close", OnClick = () => {}}
            });
        });
    }
}
