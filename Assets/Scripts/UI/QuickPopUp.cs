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

    private Queue<PopUpData> PopUpsToShow = new Queue<PopUpData>();

    public void SetData(Sprite icon, string text) {

        // Note: Text can be rich text
        if (!alreadyActive) {
            PositionLerper.StartLerping();
            AlphaLerper.EndReverseLerping();

            alreadyActive = true;

            Image.sprite = icon;
            Text.text = text;
        }
        else {
            PopUpsToShow.Enqueue(new PopUpData() { Icon = icon, Text = text });
        }
    }

    private void ProcessNext() {
        if (PopUpsToShow.Count == 0) {
            PositionLerper.StartReverseLerp();
            AlphaLerper.StartLerping();
            alreadyActive = false;
            return;
        }

        AlphaLerper.StartLerping(() => {
            PopUpData data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;

            AlphaLerper.StartReverseLerp();
        });
    }

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
