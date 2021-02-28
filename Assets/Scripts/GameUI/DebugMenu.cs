using G4AW2.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour {

    #region Add Enemy

    public TMP_InputField EnemyLevel;
    public TMP_Dropdown EnemyDropdown;
    public FollowerManager _followers;

    public void PopulateDropdown() {
        List<TMP_Dropdown.OptionData> dropdownItems = new List<TMP_Dropdown.OptionData>();
        foreach (var follower in _followers.AllFollowers) {
            dropdownItems.Add(new TMP_Dropdown.OptionData(follower.DisplayName, follower.Portrait));
        }

        EnemyDropdown.ClearOptions();
        EnemyDropdown.AddOptions(dropdownItems);
    }

    public void DropEnemy() {

	    var level = int.Parse(EnemyLevel.text);
	    var config = _followers.AllFollowers.ElementAt(EnemyDropdown.value);
        _followers.DebugAdd(config, level);
    }

    #endregion

    // Use this for initialization
    void Start () {
		PopulateDropdown();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
