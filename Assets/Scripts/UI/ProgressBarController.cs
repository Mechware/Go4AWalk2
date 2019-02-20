using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour {

    [Header("Values")]
    public IntReference Max;
    public IntReference Current;

    [Header("UI")]
    public Image ProgressBarFill;

	void OnEnable() {
	    if(!Max.UseConstant)
	        Max.Variable.OnChange.AddListener(UpdateUI);
	    if(!Current.UseConstant)
	        Current.Variable.OnChange.AddListener(UpdateUI);
    }

	void OnDisable() {
		if (!Max.UseConstant)
			Max.Variable.OnChange.RemoveListener(UpdateUI);
		if (!Current.UseConstant)
			Current.Variable.OnChange.RemoveListener(UpdateUI);
	}

	void Start() {
        UpdateUI();
	}

	private void UpdateUI(int i ) { UpdateUI();}

    public void SetMax( int Max ) {
        this.Max.Value = Max;
        UpdateUI();
    }

    public void SetCurrent( int Current ) {
        this.Current.Value = Current;
        UpdateUI();
    }

    [ContextMenu("Update")]
    public void UpdateUI() {
        Vector3 scale = ProgressBarFill.rectTransform.localScale;
        if (Max == 0) {
            return;
        }
        scale.x = Mathf.Clamp01((float)Current / Max);
        ProgressBarFill.rectTransform.localScale = scale;
    }
}
