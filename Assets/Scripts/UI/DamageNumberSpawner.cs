using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour {

    public GameObject DamageNumberPrefab;
    public Transform damageNumberParent;

    public void SpawnNumber(int number) {
        GameObject damageNumber = GameObject.Instantiate(DamageNumberPrefab, damageNumberParent);
        damageNumber.GetComponent<TextMeshProUGUI>().SetText(number.ToString());
        
    }
}
