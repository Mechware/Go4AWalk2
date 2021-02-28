using G4AW2;
using G4AW2.Component.UI;
using G4AW2.Managers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour {

	public GameObject ListItemPrefab;
	public GameObject ListParent;

	[SerializeField] private QuestManager _quests;
	[SerializeField] private PopUp _popUp;
    public GameObject Journal;

    private List<GameObject> questItems = new List<GameObject>();

    public void RefreshList() {
        Clear();
        SaveGame.SaveData.CurrentQuests.ForEach(AddItem);
    }

	public void AddItem(QuestSaveData info) {
		var gameobject = GameObject.Instantiate(ListItemPrefab);
		gameobject.transform.SetParent(ListParent.transform, false);

		questItems.Add(gameobject);
		gameobject.GetComponent<Button>().onClick.RemoveAllListeners();
        var instance = new QuestInstance(info);
        gameobject.GetComponent<Button>().onClick.AddListener(() =>
        {
            var q = instance;
            _popUp.SetPopUpNew($"{q.Config.DisplayName}\nWhat would you like to do?", new[] { "Set Active", "Remove", "Cancel" },
                    new Action[] {
                () => {
                    // Set Active
                    if (!_quests.CurrentQuest.SaveData.Complete) {
                        _popUp.SetPopUpNew(
                            "Are you sure you want to switch quests? You will lose all progress in this one.",
                            new[] {"Yep", "Nope"}, new Action[] {
                                () => {
                                    _quests.SetCurrentQuest(q);
                                },
                                () => { }
                            });
                    }
                    else {
                        // You've already completed the quest
                        _quests.SetCurrentQuest(q);
                    }
                    Journal.SetActive(false);

                },
                () => {
                    // Remove
                    SaveGame.SaveData.CurrentQuests.Remove(q.SaveData);
                },
                () => {
                    // Cancel
                }
            });

        });

		SetContent(instance, gameobject);
	}

	private void SetContent(QuestInstance info, GameObject go) {
		go.GetComponentInChildren<TextMeshProUGUI>().text = info.Config.DisplayName;
	}

	public void Clear() {
		questItems.ForEach(Destroy);
		questItems.Clear();
	}

}
