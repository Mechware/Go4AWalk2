using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("Use canvas group")]
public class AlphaOfAllChildren : MonoBehaviour {

	public void SetAlphaOfAllChildren(float alpha) {
		SetAlphaOfAllChildren(gameObject, alpha);
	}

	public static void SetAlphaOfAllChildren(GameObject rt, float alpha, Color? tint = null) {
        Image[] ims = rt.GetComponentsInChildren<Image>();
        foreach(Image im in ims) {
            Color c = im.color;
            if (tint.HasValue) c = tint.Value;
            c.a = alpha;
            im.color = c;
        }

        TextMeshProUGUI []texts = rt.GetComponentsInChildren<TextMeshProUGUI>();
	    foreach(TextMeshProUGUI text in texts) {
	        Color c = text.faceColor;
	        if (tint.HasValue) c = tint.Value;
	        c.a = alpha;
	        text.faceColor = c;
	        Color o = text.outlineColor;
	        o.a = alpha;
	        text.outlineColor = o;
	    }
    }
	
    public void SetAlphaOfAllImages(float alpha) {
        Image[] ims = GetComponentsInChildren<Image>();
        foreach(Image im in ims) {
            Color c = im.color;
            c.a = alpha;
            im.color = c;
        }
    }
}
