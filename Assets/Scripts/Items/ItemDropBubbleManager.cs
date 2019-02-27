using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class ItemDropBubbleManager : MonoBehaviour {

	public GameObject ItemDropperPrefab;

	public float SpawnDelay;

	public UnityEventItem OnItemClick;
	public UnityEvent OnFinished;

	public void AddItems( IEnumerable<Item> items ) {
		List<Item> itemsList = items.ToList();
        if(itemsList.Count == 0) {
            OnFinished.Invoke();
            return;
        }
		StartCoroutine(ShootItems(itemsList));
	}

	private int onScreenItems = 0;
	private bool finished = true;

	private IEnumerator ShootItems( IEnumerable<Item> items ) {
		finished = false;
		foreach (Item it in items) {
			yield return new WaitForSeconds(SpawnDelay);
			GameObject itemBubble = GameObject.Instantiate(ItemDropperPrefab, Vector3.zero, Quaternion.identity, transform);
            itemBubble.transform.localPosition = new Vector3(0, 0, 0);
            itemBubble.GetComponent<ItemDropBubble>().SetData(it, OnClick);
			itemBubble.GetComponent<ItemDropBubble>().Shoot();
			onScreenItems++;
		}
		finished = true;
	}

	private void OnClick(ItemDropBubble it) {
		OnItemClick.Invoke(it.Item);
		Destroy(it.gameObject);
		onScreenItems--;
		if (onScreenItems == 0 && finished) {
			OnFinished.Invoke();
		}
	}

	[Header("Debug")] public ItemDropper Dropper;
#if UNITY_EDITOR
	[ContextMenu("Drop Items")]
	public void DropItems() {
		AddItems(Dropper.GetItems(true));
	}

#endif
}
