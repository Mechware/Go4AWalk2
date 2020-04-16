using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Followers;
using TMPro;
using UnityEngine;

public class DebugMenu : MonoBehaviour {

    #region Add Enemy

    public TMP_InputField EnemyLevel;
    public TMP_Dropdown EnemyDropdown;

    public void PopulateDropdown() {
        List<TMP_Dropdown.OptionData> dropdownItems = new List<TMP_Dropdown.OptionData>();
        foreach (var follower in Configs.Instance.Followers) {
            dropdownItems.Add(new TMP_Dropdown.OptionData(follower.DisplayName, follower.Portrait));
        }

        EnemyDropdown.ClearOptions();
        EnemyDropdown.AddOptions(dropdownItems);
    }

    public void DropEnemy() {

	    var level = int.Parse(EnemyLevel.text);
	    var config = Configs.Instance.Followers.ElementAt(EnemyDropdown.value);
        FollowerInstance fi = FollowerFactory.GetInstance(config, level);
        // Drop enemy?
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
