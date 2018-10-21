using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

public class FollowerPopulator : MonoBehaviour {

	public PersistentSetFollowerData From;
	public RuntimeSetFollowerData To;

	void Awake() {
		To.Value.Clear();
		To.Value.AddRange(From.List);
	}
}
