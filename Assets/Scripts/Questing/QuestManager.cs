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
	public RuntimeSetQuest CurrentQuests;

	[Header("Events")]
	public UnityEvent OnMenuOpen;
	public UnityEvent OnMenuClose;
	public UnityEventQuest AreaQuestChanged;
	public UnityEvent ResetQuestState;

	public void LoadQuestFromID() {
		SetCurrentQuest(AllQuests.First(q => q.ID == CurrentQuestId));
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
		CurrentQuests.Value.ForEach(QuestList.AddItem);
	}

	public void CloseQuestMenu() {
		OnMenuClose.Invoke();
	}

	private bool receivedEndPopUp = false;

	public void PlayerMoved(float distanceMoved) {
		DistanceWalkedInQuest.Value += distanceMoved;

		if (CurrentQuest.TotalDistanceToWalk == -1)
			return; // YOU CAN NEVER FINISH MWHAHHAAHA

		if (DistanceWalkedInQuest >= CurrentQuest.TotalDistanceToWalk && !receivedEndPopUp) {
			QuestDialogUI.SetConversation(CurrentQuest.EndConversation, AdvanceQuest);
			receivedEndPopUp = true;
		}
	}

	public void AdvanceQuest() {

		if (CurrentQuest.NextQuest == null) {
			PopUp.SetPopUp(
				"You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
				new[] {"ok"}, new Action[] {
					() => { }
				});
			return;
		}

		ResetQuestState.Invoke();
		CurrentQuests.Remove(CurrentQuest);
		SetCurrentQuest(CurrentQuest.NextQuest);
	}

	public void QuestClicked(Quest q) {

		if (q == CurrentQuest) {
			PopUp.SetPopUp("This is your current quest.", new[] {"Cool", "Nice."}, new Action[] {() => { }, () => { }});
			return;
		}
		if (CurrentQuest.TotalDistanceToWalk > DistanceWalkedInQuest) {
			PopUp.SetPopUp("Are you sure you want to switch quests? You will lose all progress in this one.",
				new[] {"Yep", "Nope"}, new Action[] {
					() => {
						ResetQuestState.Invoke();
						SetCurrentQuest(q);
					},
					() => { }
				});
		}
		else {
			// You've already completed the quest
			CurrentQuests.Remove(CurrentQuest);
			ResetQuestState.Invoke();
			SetCurrentQuest(q);
		}
		
	}

	public void SetCurrentQuest(Quest quest) {

		receivedEndPopUp = false;

		if (CurrentQuests.Value.FirstOrDefault(q => q.ID == quest.ID) == null) {
			CurrentQuests.Add(quest);
		}

		CurrentQuest = quest;
		CurrentQuestId.Value = quest.ID;
		AreaQuestChanged.Invoke(quest);
		if (DistanceWalkedInQuest <= 0) {
			QuestDialogUI.SetConversation(quest.StartConversation, () => { });
		}

		PlayerMoved(0);
	}
}
