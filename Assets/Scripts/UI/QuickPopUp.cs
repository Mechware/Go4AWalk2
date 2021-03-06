using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPopUp : MonoBehaviour {

    public Image Image;
    public TextMeshProUGUI Text;
    public ClickReceiver Button;

    [SerializeField] private Animator animator;
    private Queue<PopUpData> PopUpsToShow = new Queue<PopUpData>();

    private static readonly int NumberOfPopUpsHash = Animator.StringToHash("NumberOfPopUps");
    private static readonly int DisabledHash = Animator.StringToHash("Disabled");
    private static readonly int TappedHash = Animator.StringToHash("Tapped");

    private enum State
    {
        Active,
        Enabled,
        AlphaRunning
    }

    private class PopUpData {
        public Sprite Icon;
        public string Text;
    }

    private void Awake()
    {
        Button.MouseClick.AddListener(() =>
        {
            animator.SetTrigger(TappedHash);
        });
    }

    public void Enable()
    {
        animator.SetBool(DisabledHash, false);
    }
    public void Disable()
    {
        animator.SetBool(DisabledHash, true);
    }

    public void Show(Sprite icon, string text) {
        PopUpsToShow.Enqueue(new PopUpData() { Icon = icon, Text = text });
        animator.SetInteger(NumberOfPopUpsHash, PopUpsToShow.Count);
    }
    public void AdvancePopUps()
    {
        if(PopUpsToShow.Count == 0)
        {
            Debug.LogError("Tried to advance pop ups when none are left");
            return;
        }

        PopUpData data = PopUpsToShow.Dequeue();
        Image.sprite = data.Icon;
        Text.text = data.Text;

        animator.SetInteger(NumberOfPopUpsHash, PopUpsToShow.Count);
        animator.ResetTrigger(TappedHash);
    }

    [SerializeField] private Sprite TestSprite;
    [SerializeField] private string TestString;
    [ContextMenu("Add test")]
    public void AddTest()
    {
        Show(TestSprite, TestString);
    }
}
