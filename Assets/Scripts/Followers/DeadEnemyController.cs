using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.UI;

public class DeadEnemyController : MonoBehaviour {

    [Obsolete("Singleton")] public static DeadEnemyController Instance;
    
    public int EndingPosition = -527;

    public GameObject DeadEnemyPrefab;
    public Transform DeadEnemyParent;

    private ObjectPrefabPool DeadEnemies;

    void Awake() {
        Instance = this;
        DeadEnemies = new ObjectPrefabPool(DeadEnemyPrefab, DeadEnemyParent);
    }

    public void ClearEnemies() {
        StopAllCoroutines();
        DeadEnemies.Reset();
    }

    public void AddDeadEnemy(float x, float y, EnemyData s) {

        GameObject go = DeadEnemies.GetObject();

        go.GetComponent<Image>().sprite = s.DeadSprite;
        RectTransform rt = ((RectTransform) go.transform);
        Vector2 r = rt.sizeDelta;
        r.x = s.SizeOfSprite.x;
        r.y = s.SizeOfSprite.y;
        rt.sizeDelta = r;

        Vector3 pos = rt.anchoredPosition;
        pos.x = x;
        pos.y = y;
        rt.anchoredPosition = pos;

        StartCoroutine(Fade(go));
    }

    private List<GameObject> _toReturn = new List<GameObject>();
    public void Scroll(float distance)
    {
        foreach(var go in DeadEnemies.InUse)
        {
            RectTransform rt = go.GetComponent<RectTransform>();
            Vector3 pos = rt.anchoredPosition;
            pos.x -= distance;
            rt.anchoredPosition = pos;

            if(rt.anchoredPosition.x <= EndingPosition)
            {
                _toReturn.Add(go);
            }
        }

        foreach(var enemy in _toReturn) DeadEnemies.Return(enemy);
        _toReturn.Clear();

    }

    [Header("Fade out")]
    [SerializeField] private float _minGray = 0.6f;
    [SerializeField] private float _fadeRate = 0.1f;
    [SerializeField] private float _fadeDelay = 2;

    IEnumerator Fade(GameObject go) {

        Image im = go.GetComponent<Image>();
        float grayness = 1f;
        im.color = new Color(grayness, grayness, grayness, 1);

        yield return new WaitForSeconds(_fadeDelay);

        while(grayness > _minGray) {
            im.color = new Color(grayness, grayness, grayness, 1);
            grayness -= _fadeRate * Time.deltaTime;
            yield return null;
        }
    }
}
