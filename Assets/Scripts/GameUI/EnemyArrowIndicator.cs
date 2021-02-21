using CustomEvents;
using G4AW2.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyArrowIndicator : MonoBehaviour {

    [Obsolete("Pass this in during initialization")] public RuntimeSetFollowerData Followers;
    public Image Arrow;
    public TextMeshProUGUI NumberofFollowersText;

    public bool OnMainScreen = true;
    public bool HasFollowers = true;

    private bool _enabled = true;

    private void Awake() {
        Followers.OnChange.AddListener(FollowersChanged);
    }

    public void FollowersChanged(FollowerData data) {
        NumberofFollowersText.text = "x" + Followers.Value.Count;
        HasFollowers = Followers.Value.Count > 0;

        RefreshShowing();
    }

    public void SetOnMainScreen(bool val) {
        OnMainScreen = val;
        RefreshShowing();
    }

    public void Disable()
    {
        _enabled = false;
        Arrow.enabled = false;
    }

    public void Enable()
    {
        _enabled = true;
        RefreshShowing();
    }

    private void RefreshShowing()
    {
        bool show = OnMainScreen && HasFollowers && _enabled;
        Arrow.enabled = show;
        NumberofFollowersText.gameObject.SetActive(show);
    }
}
