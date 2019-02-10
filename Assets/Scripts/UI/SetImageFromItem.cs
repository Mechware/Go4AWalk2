﻿using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageFromItem : MonoBehaviour {

	public void SetImage(Item a)
    {
        GetComponent<Image>().sprite = a.image;
    }
}
