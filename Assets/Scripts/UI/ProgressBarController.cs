using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour {

    [Header("Values")]
    public int Max;
    public int Min;

    public int Current;

    [Header("UI")]
    public Image ProgressBarFill;

    public void SetMax( int Max ) {
        this.Max = Max;
        UpdateUI();
    }

    public void SetMin( int Min ) {
        this.Min = Min;
        UpdateUI();
    }

    public void SetCurrent( int Current ) {
        this.Current = Current;
        UpdateUI();
    }

    [ContextMenu("Update")]
    public void UpdateUI() {
        Vector3 scale = ProgressBarFill.rectTransform.localScale;
        if (Max - Min == 0) {
            Debug.LogWarning("Max - min is zero. Object: " + name);
            return;
        }
        scale.x = Mathf.Clamp01((float)(Current - Min) / (Max - Min));
        ProgressBarFill.rectTransform.localScale = scale;
    }
}
