using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPopUp : MonoBehaviour {

    public RobustLerper PositionLerper;
    public RobustLerper AlphaLerper;

    public Image Image;
    public TextMeshProUGUI Text;

    private bool alreadyActive = false;

    private class PopUpData {
        public Sprite Icon;
        public string Text;
    }

    void Awake() {
        instance = this;
    }

    private static QuickPopUp instance;

    public static void Show(Sprite icon, string text) {
        instance.SetData(icon, text);
    }

    private Queue<PopUpData> PopUpsToShow = new Queue<PopUpData>();

    public void SetData(Sprite icon, string text) {

        // Note: Text can be rich text
        if (!alreadyActive) {
            PositionLerper.StartLerping();
            AlphaLerper.StartReverseLerp();

            alreadyActive = true;

            Image.sprite = icon;
            Text.text = text;
        }
        else {
            PopUpsToShow.Enqueue(new PopUpData() { Icon = icon, Text = text });
        }
    }

    private void ProcessNext() {

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

    private bool alphaLerpRunning = false;

    public void PopUpClicked() {
        ProcessNext();
    }



    #region Debug

    public Sprite TestSprite1;
    public Sprite TestSprite2;
    public Sprite TestSprite3;
    [Multiline]
    public string TestText1;
    [Multiline]
    public string TestText2;
    [Multiline]
    public string TestText3;

    [ContextMenu("Debug Stuff")]
    public void DebugStuff() {
        SetData(TestSprite1, TestText1);
        SetData(TestSprite2, TestText2);
        SetData(TestSprite3, TestText3);
    }

    #endregion
}
