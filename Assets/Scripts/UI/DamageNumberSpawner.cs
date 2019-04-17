using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour {

    public GameObject DamageNumberPrefab;
    public Transform damageNumberParent;

    private ObjectPrefabPool pool;

    void Awake() {
        pool = new ObjectPrefabPool(DamageNumberPrefab, damageNumberParent, 5);
    }

    void OnEnable() {
        pool.Reset();
    }

    public void SpawnNumber(int number, Color c) {

        GameObject damageNumber = pool.GetObject();
        ((RectTransform) damageNumber.transform).anchoredPosition = new Vector2(0,0);
        damageNumber.GetComponent<TextMeshProUGUI>().SetText(number.ToString());
        damageNumber.GetComponent<TextMeshProUGUI>().faceColor = c;
        damageNumber.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0, 2, false);

        Timer.StartTimer(this, 5, () => {pool.Return(damageNumber);});
    }

#if UNITY_EDITOR
    [ContextMenu("Spawn Test")]
    public void SpawnNumberTest() {
        SpawnNumber(Random.Range(1,1000), Color.black);
    }
#endif
}
