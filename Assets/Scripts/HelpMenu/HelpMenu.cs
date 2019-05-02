using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour {

    public HelpMenuItem[] HelpItems;
    public GameObject HelpItemPrefab;
    public Transform HelpItemParent;

    public HelpMenuPopUp PopUp;

	// Use this for initialization
	void Start () {
	    foreach (var item in HelpItems) {
	        var go = Instantiate(HelpItemPrefab, HelpItemParent);
	        go.GetComponentInChildren<TextMeshProUGUI>().text = item.DisplayName;
            SetClickHandler(go.GetComponent<Button>(), item);
	    }
	}

    void SetClickHandler(Button b, HelpMenuItem item) {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => {
            PopUp.ShowItem(item);
        });
    }

	// Update is called once per frame
	void Update () {
		
	}
}
