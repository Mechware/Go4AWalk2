using G4AW2.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour {

	public GameObject ListItemPrefab;
	public GameObject ListParent;

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
		gameobject.GetComponent<Button>().onClick.AddListener(() => QuestManager.Instance.QuestClicked(instance));

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
