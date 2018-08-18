using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCatcher : Graphic {
	protected override void Awake() {
		color = new Color(0, 0, 0, 0);
	}
	protected override void OnEnable() {
		color = new Color(0, 0, 0, 0);
	}
}
