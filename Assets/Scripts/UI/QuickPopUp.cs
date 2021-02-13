using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPopUp : MonoBehaviour {

    public RobustLerper PositionLerper;
    public RobustLerper AlphaLerper;
    public RobustLerperSerialized ShakeLerper;

    public Image Image;
    public TextMeshProUGUI Text;

    private bool alreadyActive = false;
    private Queue<PopUpData> PopUpsToShow = new Queue<PopUpData>();
    private bool alphaLerpRunning = false;

    public bool IsMainInstance = false;

    private bool _active = true;

    private class PopUpData {
        public Sprite Icon;
        public string Text;
    }

    public void Enable()
    {
        _active = true;
        ProcessNext();
    }
    public void Disable()
    {
        _active = false;
        Hide();
    }

    private void Hide()
    {
        if (!alreadyActive) return;

        PositionLerper.StartReverseLerp();
        AlphaLerper.StartLerping();
        alreadyActive = false;
        alphaLerpRunning = false;
    }

    public void ShowSprite(Sprite icon, string text) {
        if(alreadyActive) {
            Shake();
        }
        AddToQueue(icon, text);
        StartPopUp();
    }

    public void Shake() {
        ShakeLerper.StartLerping();
    }

    private void AddToQueue(Sprite icon, string text) {
        PopUpsToShow.Enqueue(new PopUpData() { Icon = icon, Text = text });
    }

    private void StartPopUp() {

        if (!_active || alreadyActive || PopUpsToShow.Count == 0) return;

        PositionLerper.StartLerping();
        AlphaLerper.StartReverseLerp();

        alreadyActive = true;

        var data = PopUpsToShow.Dequeue();
        Image.sprite = data.Icon;
        Text.text = data.Text;
    }

    [Obsolete("TODO: Replace this with a coroutine")]
    private void ProcessNext() {

        if(!_active)
            return;

        if(alphaLerpRunning) {
            PopUpData data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;
        }

        if (PopUpsToShow.Count == 0) {
            PositionLerper.StartReverseLerp();
            AlphaLerper.StartLerping();
            alreadyActive = false;
            alphaLerpRunning = false;
            return;
        }

        alphaLerpRunning = true;
        AlphaLerper.StartLerping(() => {
            alphaLerpRunning = false;
            PopUpData data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;

            AlphaLerper.StartReverseLerp();
        });
    }
    public void PopUpClicked() {

        ProcessNext();
    }
}
