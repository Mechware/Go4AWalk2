using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaOfAllChildren : MonoBehaviour {

	public void SetAlphaOfAllChildren(float alpha) {
        Image[] ims = GetComponentsInChildren<Image>();
        foreach(Image im in ims) {
            Color c = im.color;
            c.a = alpha;
            im.color = c;
        }
    }
}
