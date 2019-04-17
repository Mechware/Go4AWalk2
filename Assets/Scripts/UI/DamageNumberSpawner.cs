using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour {

    public GameObject DamageNumberPrefab;
    public Transform damageNumberParent;



    public void SpawnNumber(int number, Color c) {
        GameObject damageNumber = GameObject.Instantiate(DamageNumberPrefab, damageNumberParent);
        damageNumber.GetComponent<TextMeshProUGUI>().SetText(number.ToString());
        damageNumber.GetComponent<TextMeshProUGUI>().faceColor = c;
    }

#if UNITY_EDITOR
    [ContextMenu("Spawn Test")]
    public void SpawnNumberTest() {
        SpawnNumber(Random.Range(1,1000), Color.black);
    }
#endif
}
