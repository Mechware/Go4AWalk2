using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour {
    public RectTransform MasteryFill;
    public TextMeshProUGUI MasteryBeginText;
    public TextMeshProUGUI MasteryEndText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        var weapon = DataManager.Instance.Player.Weapon.Value;
        
        float currentDamage = weapon.RawDamage;
        
        if(weapon.Mastery == 99) {
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(1);
            MasteryBeginText.text = $"{weapon.Mastery} ({currentDamage})";
            MasteryEndText.text = $"";
        }
        else {
            float masteryProgress = weapon.RawMastery - Mathf.Floor(weapon.RawMastery);
            float nextLevelDamage = weapon.GetDamage(mastery: weapon.Mastery+1);
            MasteryFill.anchorMax = MasteryFill.anchorMax.SetX(masteryProgress);
            MasteryBeginText.text = $"{weapon.Mastery} ({currentDamage})";
            MasteryEndText.text = $"{weapon.Mastery+1} ({nextLevelDamage})";
        }
        
        
        
    }
}
