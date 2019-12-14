using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Questing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour {

	public GameObject ListItemPrefab;
	public GameObject ListParent;

	private List<GameObject> questItems = new List<GameObject>();

    public void RefreshList() {
        Clear();
        QuestManager.Instance.ActiveQuests.ForEach(AddItem);
    }

	public void AddItem(QuestInstance info) {
		var gameobject = GameObject.Instantiate(ListItemPrefab);
		gameobject.transform.SetParent(ListParent.transform, false);

		questItems.Add(gameobject);
		gameobject.GetComponent<Button>().onClick.RemoveAllListeners();
		gameobject.GetComponent<Button>().onClick.AddListener(() => QuestManager.Instance.QuestClicked(info));

		SetContent(info, gameobject);
	}

	private void SetContent(QuestInstance info, GameObject go) {
		go.GetComponentInChildren<TextMeshProUGUI>().text = info.Config.DisplayName;
	}

	public void Clear() {
		questItems.ForEach(Destroy);
		questItems.Clear();
	}

}
