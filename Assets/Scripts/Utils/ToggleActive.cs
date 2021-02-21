using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Smelly")]
public class ToggleActive : MonoBehaviour {

    public void Toggle(GameObject go) {
        go.SetActive(!go.activeSelf);
    }
}
