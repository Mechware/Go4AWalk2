using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class ItemDropBubbleManager : MonoBehaviour {

    public Inventory Inventory;
    public WeaponVariable PlayerWeapon;

	public GameObject ItemDropperPrefab;

	public float SpawnDelay;

    private ObjectPrefabPool Pool;

    void Awake() {
        Pool = new ObjectPrefabPool(ItemDropperPrefab, transform, 5);
    }

    public void AddItems(IEnumerable<Item> items, Action onClick) {
        List<Item> itemsList = items.ToList();
        StartCoroutine(ShootItems(itemsList, onClick));
    }

	public void AddItems( IEnumerable<Item> items ) {
		List<Item> itemsList = items.ToList();
		StartCoroutine(ShootItems(itemsList, null));
	}

	private IEnumerator ShootItems( IEnumerable<Item> items, Action onClick ) {
		foreach (Item it in items) {
			yield return new WaitForSeconds(SpawnDelay);
		    GameObject itemBubble = Pool.GetObject();
		    itemBubble.transform.localPosition = Vector3.zero;
            itemBubble.transform.rotation = Quaternion.identity;
            itemBubble.GetComponent<ItemDropBubble>().SetData(it, (i) => OnClick(i, onClick));
			itemBubble.GetComponent<ItemDropBubble>().Shoot();
		}
	}

    public WeaponUI WeaponUI;

	private void OnClick(ItemDropBubble it, Action onClick) {

	    if (it.Item is Weapon) {

	        WeaponUI.SetWeaponWithDefaults((Weapon)it.Item, () => {
	            Pool.Return(it.gameObject);
	            onClick?.Invoke();
            });
	        return;
	    }

        Pool.Return(it.gameObject);
	    onClick?.Invoke();
    }

    public void Clear() {
        if (Pool.InUse.Count == 0) return;

        string loot = "";
        Sprite s = null;
        foreach (var obj in Pool.InUse.ToArray()) {
            var bubble = obj.GetComponent<ItemDropBubble>();
            bubble.OnPointerClick(null);
            loot = bubble.Item.GetName() + "\n";
            if(s == null) s = bubble.Item.Image;
        }
        QuickPopUp.Show(s, "<size=150%>Auto Collected Loot</size>\nLoot was auto collected for you. The following was picked up:\n" + loot);

        Pool.Reset();
    }

    [Header("Debug")] public ItemDropper Dropper;
#if UNITY_EDITOR
	[ContextMenu("Drop Items")]
	public void DropItems() {
		AddItems(Dropper.GetItems(true));
	}

#endif
}
