using CustomEvents;
using G4AW2.Questing;
using Sirenix.Utilities;
using System.Linq;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(QuestListUI))]
public class QuestManager : MonoBehaviour {

#if UNITY_EDITOR
	public PersistentSetQuest AllQuestsTest;
#endif

	public RuntimeSetQuest AllQuests;
	public AreaQuest CurrentAreaQuest;
	public UnityEvent OnMenuOpen;
	public UnityEvent OnMenuClose;
	public AreaManager AreaManager;

	private QuestListUI QuestList;

	void Awake() {
		QuestList = GetComponent<QuestListUI>();
	}

	void Start() {
		AreaManager.SetArea(CurrentAreaQuest.Area);
#if UNITY_EDITOR
		AllQuestsTest.List.ForEach(AllQuests.Add);
#endif
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

	public void GPSUpdate(float distanceMoved) {
		AllQuests.Value.Where(q => q.Completed == false).ForEach(q => q.GPSUpdate(distanceMoved));
	}

	public void QuestClicked(Quest q) {
		print("Quest clicked: " + q);
	}

}
