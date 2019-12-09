using CustomEvents;
using G4AW2.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Followers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyArrowIndicator : MonoBehaviour {

    public static EnemyArrowIndicator Instance;
    
    public Image Arrow;
    public TextMeshProUGUI NumberofFollowersText;

    public bool OnMainScreen = true;
    public bool HasFollowers = true;

    private void Awake() {
        Instance = this;
        FollowerManager.Instance.FollowerAdded += FollowersChanged;
    }

    public void FollowersChanged(FollowerInstance instance) {
        HasFollowers = FollowerManager.Instance.Followers.Count > 0;

        Arrow.enabled = OnMainScreen && HasFollowers;
        NumberofFollowersText.gameObject.SetActive(OnMainScreen && HasFollowers);
       
        NumberofFollowersText.text = "x" + FollowerManager.Instance.Followers.Count;
    }

    public void SetOnMainScreen(bool val) {
        OnMainScreen = val;

        Arrow.enabled = OnMainScreen && HasFollowers;
        NumberofFollowersText.gameObject.SetActive(OnMainScreen && HasFollowers);

    }
}
