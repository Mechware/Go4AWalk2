using G4AW2.Component.UI;
using G4AW2.Data;
using G4AW2.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiningPoints : MonoBehaviour {

    [SerializeField] private int ReturnToPoolXValue;

    [SerializeField] private GameObject PointPrefab;
    [SerializeField] private Transform Parent;

    [SerializeField] private bool IsScrolling;

    [SerializeField] private QuestManager _quests;
    [SerializeField] private ItemManager _items;
    [SerializeField] private ScrollingImages _backgroundImages;

    public Action<ItemInstance> OnItemReceived;

    private ObjectPrefabPool Pool;

    private List<MiningPoint> areaPoints = new List<MiningPoint>();
    private List<float> nextSpawnDistance = new List<float>();

    private Area currentArea;

    public void SetQuest(QuestConfig a) {
        if (currentArea != a.Area) {
            currentArea = a.Area;
            Pool.Reset();
        }

        areaPoints = a.MiningPoints;
        nextSpawnDistance.Clear();
        foreach(var point in areaPoints) {
            nextSpawnDistance.Add(Random.Range(point.MinDistanceBetween, point.MaxDistanceBetween));
        }
    }

    void Awake() {
        Pool = new ObjectPrefabPool(PointPrefab, Parent, 3);
        _quests.QuestStarted += q => SetQuest(q.Config);
        _backgroundImages.OnScrolled += Scroll;
        OnItemReceived += _items.OnItemObtained;
    }

    public void Scroll(float distance)
    {
        for (int i = 0; i < nextSpawnDistance.Count; i++)
        {
            nextSpawnDistance[i] -= distance;
            if (nextSpawnDistance[i] <= 0)
            {
                MiningPoint point = areaPoints[i];
                GameObject go = Pool.GetObject();
                Image im = go.GetComponent<Image>();
                im.sprite = point.Image;
                im.SetNativeSize();
                AddListener(go.GetComponent<ClickReceiver>(), point);

                Vector3 pos = go.transform.localPosition;
                pos.x = 79;
                go.transform.localPosition = pos;

                nextSpawnDistance[i] = Random.Range(point.MinDistanceBetween, point.MaxDistanceBetween);
            }
        }

        foreach (var point in Pool.InUse.ToArray())
        {
            Vector3 pos = point.transform.localPosition;
            pos.x -= distance;
            if (pos.x <= ReturnToPoolXValue)
            {
                Pool.Return(point);
            }

            point.transform.localPosition = pos;
        }
    }

    void AddListener(ClickReceiver cr, MiningPoint point) {
        cr.MouseClick2D.RemoveAllListeners();
        cr.MouseClick2D.AddListener((v) => {

            if(InteractionCoordinator.Instance.Fighting)
                return;

            var items = point.Drops.GetItems(0, _items);
            string itemText = "";
            foreach (var item in items) {
                itemText += $"A {item.GetName()}\n";
                OnItemReceived?.Invoke(item);
            }
            if (items.Count == 0) {
                PopUp.SetPopUp("The mining point breaks apart and you get nothing :(", new[] {"Shucks!"}, new Action[] {() => { }});
            } else {
                PopUp.SetPopUp("You got: " + itemText, new[] {"Awesome!"}, new Action[] {() => { }});
            }
            Pool.Return(cr.gameObject);
        });
    }
}
