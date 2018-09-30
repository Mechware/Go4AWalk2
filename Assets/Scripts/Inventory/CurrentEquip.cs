using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEquip : MonoBehaviour {


    //make this listen to event that passes data too for smoothness. 

    public GameObject hat, weapon, armor, boots;
    public Player player;

	// Use this for initialization
	void Start () {
        updateDisplay();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void updateDisplay()
    {
        if (player.hat != null) hat.GetComponent<Image>().sprite = player.hat.image;
        if (player.weapon != null) weapon.GetComponent<Image>().sprite = player.weapon.image;
        if (player.armor != null) armor.GetComponent<Image>().sprite = player.armor.image;
        if (player.boots != null) boots.GetComponent<Image>().sprite = player.boots.image;
    }
}
