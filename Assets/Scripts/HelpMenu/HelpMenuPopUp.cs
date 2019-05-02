using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenuPopUp : MonoBehaviour {

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Text;

    public Transform SeeAlsoParent;
    public GameObject SeeAlsoPrefab;

    private ObjectPrefabPool pool = null;
    private ObjectPrefabPool Pool {get{
            if(pool == null) pool = new ObjectPrefabPool(SeeAlsoPrefab, SeeAlsoParent, 8);
        return pool;
    }}


    public void ShowItem(HelpMenuItem item) {
        gameObject.SetActive(true);

        
        Title.text = "Help: " + item.DisplayName;
        Text.text = item.Description;

        Pool.Reset();

        foreach (var seeAlso in item.SeeAlso) {
            var go = Pool.GetObject();
            go.GetComponentInChildren<TextMeshProUGUI>().text = seeAlso.DisplayName;
            SetButton(go.GetComponent<Button>(), seeAlso);
        }
    }

    private void SetButton(Button b, HelpMenuItem it) {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => {
            ShowItem(it);
        });
    }
}
