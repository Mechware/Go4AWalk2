using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ItemDropBubbleManager : MonoBehaviour {

	public GameObject ItemDropperPrefab;

	public float SpawnDelay;

	public UnityEventItem OnItemClick;

	public void AddItems( IEnumerable<Item> items ) {
		StartCoroutine(ShootItems(items));
	}

	private IEnumerator ShootItems( IEnumerable<Item> items ) {
		foreach (Item it in items) {
			GameObject itemBubble = GameObject.Instantiate(ItemDropperPrefab, Vector3.zero, Quaternion.identity, transform);
			itemBubble.GetComponent<ItemDropBubble>().SetData(it, OnClick);
			itemBubble.GetComponent<ItemDropBubble>().Shoot();
			yield return new WaitForSeconds(SpawnDelay);
		}
	}

	private void OnClick(ItemDropBubble it) {
		OnItemClick.Invoke(it.Item);
		Destroy(it.gameObject);
	}

#if UNITY_EDITOR
	[Header("Debug")] public ItemDropper Dropper;

	[ContextMenu("Drop Items")]
	public void DropItems() {
		AddItems(Dropper.GetItems());
	}

#endif
}
