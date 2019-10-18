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

	public static ItemDropBubbleManager Instance;
    public Inventory Inventory;
    public WeaponVariable PlayerWeapon;

	public GameObject ItemDropperPrefab;

	public float SpawnDelay;

    private ObjectPrefabPool Pool;

    void Awake() {
	    Instance = this;
        Pool = new ObjectPrefabPool(ItemDropperPrefab, transform, 5);
    }

    public void AddItems(List<Item> items, Action onClick, Action onDone) {
        StartCoroutine(ShootItems(items, onClick, onDone));
    }

    private IEnumerator ShootItems( List<Item> items, Action onClick, Action onDone) {
		
		if (Pool.InUse.Count > 0) {
			Debug.LogError("Tried to add items to item dropper when there's still items");
		}

		int count = items.Count;
		if (!items.Any()) {
			onDone?.Invoke();
			yield break;
		}

		
		
		foreach (Item it in items) {
			yield return new WaitForSeconds(SpawnDelay);
		    GameObject itemBubble = Pool.GetObject();
		    itemBubble.transform.localPosition = Vector3.zero;
            itemBubble.transform.rotation = Quaternion.identity;
            itemBubble.GetComponent<ItemDropBubble>().SetData(it, (i) => {
	            count--;
	            OnClick(i, onClick, onDone);
	            
	            if (count == 0) {
		            onDone?.Invoke();
	            }
            });
			itemBubble.GetComponent<ItemDropBubble>().Shoot();
		}
	}

    public WeaponUI WeaponUI;

	private void OnClick(ItemDropBubble it, Action onClick, Action onDone) {

        SmoothPopUpManager.ShowPopUp(it.transform.localPosition, $"<color=green>+1</color> {it.Item.GetName()}", ConfigObject.GetColorFromRarity(it.Item.Rarity));

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

    [Header("Debug")] public ItemDropper Dropper;
#if UNITY_EDITOR
	[ContextMenu("Drop Items")]
	public void DropItems() {
		AddItems(Dropper.GetItems(true), null, null);
	}

#endif
}
