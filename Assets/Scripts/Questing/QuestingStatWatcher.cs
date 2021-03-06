using G4AW2.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestingStatWatcher : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI QuestTitle;
    [SerializeField] private TextMeshProUGUI ProgressText;
    [SerializeField] private Image ProgressFill;
    [SerializeField] private SmoothPopUpManager PopUps;
    [SerializeField] private Transform SpawnPointOfNumberIncreasePopUp;

    [SerializeField] private QuestManager _quests;

    private int prevVal = -1;

    private void Awake()
    {
        _quests.QuestStarted += SetQuest;
    }

    public void SetQuest(QuestInstance q)
    {
        prevVal = _quests._currentQuest.SaveData.Progress;
    }

    public void Update()
    {
        int current = _quests._currentQuest.SaveData.Progress;
        int max = _quests._currentQuest.Config.ValueToReach;

        ProgressText.text = $"{current} / {max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float)(current / max)));
        QuestTitle.text = _quests._currentQuest.Config.DisplayName;

        if (prevVal != -1 && current > prevVal)
        {
            PopUps.ShowPopUpNew(SpawnPointOfNumberIncreasePopUp.position, "+" + (current - prevVal), Color.green, true);
            prevVal = current;
        }
    }
}
