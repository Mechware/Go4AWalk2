using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Obsolete("Use an animator")]
[RequireComponent(typeof(Image))]
public class SetImageAlpha : MonoBehaviour {

    private Image im;
    private Image Image {
        get {
            if (im == null) im = GetComponent<Image>();
            return im;
        }
    }

    public void SetAlpha(float val) {
        Color c = Image.color;
        c.a = val;
        Image.color = c;
    }
}
