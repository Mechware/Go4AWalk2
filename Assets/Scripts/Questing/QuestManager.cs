using CustomEvents;
using G4AW2.Questing;
using Sirenix.Utilities;
using System.Linq;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

	public PersistentSetQuest AllQuests;
	public Quest CurrentQuest;
	public UnityEventQuest QuestCompleted;
	public UnityEvent OnMenuOpen;
	public UnityEvent OnMenuClose;
	public UnityEventQuest AreaQuestChanged;

	public QuestListUI QuestList;

	void Awake() {
	}

	void Start() {
		// Load Current Area Quest...
		SetCurrentQuest(CurrentQuest);
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
		AllQuests.Value.ForEach(QuestList.AddItem);
	}

	public void CloseQuestMenu() {
		OnMenuClose.Invoke();
	}

	private float DistanceWalked;

	public void GPSUpdate(float distanceMoved) {
		DistanceWalked += distanceMoved;

		if (CurrentQuest.TotalDistanceToWalk == -1)
			return; // YOU CAN NEVER FINISH MWHAHHAAHA

		if (DistanceWalked >= CurrentQuest.TotalDistanceToWalk) {
			QuestCompleted.Invoke(CurrentQuest);
		}
	}

	public void QuestClicked(Quest q) {
		print("Quest clicked: " + q);
	}

	public void SetCurrentQuest(Quest q) {
		CurrentQuest = q;
		AreaQuestChanged.Invoke(q);
	}
}
