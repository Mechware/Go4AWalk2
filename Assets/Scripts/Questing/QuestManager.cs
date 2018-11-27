using System;
using CustomEvents;
using G4AW2.Questing;
using Sirenix.Utilities;
using System.Linq;
using G4AW2.Dialogue;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

	public PersistentSetQuest AllQuests;
	public Quest CurrentQuest;
	public IntReference CurrentQuestId;
	public QuestListUI QuestList;
	public FloatReference DistanceWalkedInQuest;
	public Dialogue QuestDialogUI;

	[Header("Events")]
	public UnityEventQuest QuestCompleted;
	public UnityEvent OnMenuOpen;
	public UnityEvent OnMenuClose;
	public UnityEventQuest AreaQuestChanged;

	public void LoadQuestFromID() {
		SetCurrentQuest(AllQuests.ToList().First(q => q.ID == CurrentQuestId));
	}

	private bool open = false;
	public void ToggleQuestMenu() {
		if (open) {
			CloseQuestMenu();
		}
		else {
			OpenQuestMenu();
		}
		open = !open;
	}

	public void OpenQuestMenu() {
		OnMenuOpen.Invoke();
		QuestList.Clear();
		AllQuests.ForEach(QuestList.AddItem);
	}

	public void CloseQuestMenu() {
		OnMenuClose.Invoke();
	}

	private bool completed = false;

	public void PlayerMoved(float distanceMoved) {
		if (completed) return;

		DistanceWalkedInQuest.Value += distanceMoved;

		if (CurrentQuest.TotalDistanceToWalk == -1)
			return; // YOU CAN NEVER FINISH MWHAHHAAHA

		if (DistanceWalkedInQuest >= CurrentQuest.TotalDistanceToWalk) {
			QuestDialogUI.SetConversation(CurrentQuest.EndConversation, AdvanceQuest);
		}
	}

	public void AdvanceQuest() {
		if (CurrentQuest.NextQuest == null) {
			//NANI?
			return;
		}

		DistanceWalkedInQuest.Value = 0;
		completed = false;
		SetCurrentQuest(CurrentQuest.NextQuest);
		QuestCompleted.Invoke(CurrentQuest);
	}

	public void QuestClicked(Quest q) {
		print("Quest clicked: " + q);
	}

	public void SetCurrentQuest(Quest q) {
		CurrentQuest = q;
		CurrentQuestId.Value = q.ID;
		AreaQuestChanged.Invoke(q);
		if (DistanceWalkedInQuest <= 0) {
			QuestDialogUI.SetConversation(q.StartConversation, () => { });
		}

		PlayerMoved(0);
	}
}
