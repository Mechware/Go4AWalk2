using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryToggle : MonoBehaviour {

	public int maxY = 165;
	public int minY = 25;
	public float moveSpeed = 5;

	private bool open = false;

	[Header("Events")]
	public UnityEvent OnOpenStarted;
	public UnityEvent OnOpenFinished;
	public UnityEvent OnCloseStarted;
	public UnityEvent OnCloseFinished;

	public void ToggleInventory() {
		StopAllCoroutines();
		if (!open) {
			StartCoroutine(OpenInventory());
		}
		else {
			StartCoroutine(CloseInventory());
		}
	}

	public IEnumerator OpenInventory() {

		OnOpenStarted.Invoke();

		Vector3 pos = transform.localPosition;
		while (transform.localPosition.y >= minY) {
			pos.y -= moveSpeed * Time.deltaTime;
			transform.localPosition = pos;
			yield return null;
		}
		pos.y = minY;
		transform.localPosition = pos;
		open = true;

		OnOpenFinished.Invoke();
	}

	public IEnumerator CloseInventory() {
		OnCloseStarted.Invoke();

		Vector3 pos = transform.localPosition;
		while (transform.localPosition.y <= maxY) {
			pos.y += moveSpeed * Time.deltaTime;
			transform.localPosition = pos;
			yield return null;
		}
		pos.y = maxY;
		transform.localPosition = pos;
		open = false;

		OnCloseFinished.Invoke();
	}
}
