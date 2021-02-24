using Items;
using Scripts.DoubleJump;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUi : MonoBehaviour
{
    public RectTransform RegularConsumableParent;
    public RectTransform RegularConsumablePrefab;

    [Obsolete("Singleton")] public static ConsumableUi Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        int child = 0;
        while(child < RegularConsumableParent.childCount)
            Destroy(RegularConsumableParent.GetChild(child++).gameObject);

        foreach(var data in SaveGame.SaveData.Consumables) {
            var it = Instantiate(RegularConsumablePrefab, RegularConsumableParent);
            var consumable = UiRef.Init<ConsumableItem>(it.gameObject);
            consumable.Icon.sprite = Configs.Instance.Items.First(item => item.Id == data.Id).Image;
        }

        foreach(var data in BaitBuffManager.Instance.SaveData) {
            var it = Instantiate(RegularConsumablePrefab, RegularConsumableParent);
            var consumable = UiRef.Init<ConsumableItem>(it.gameObject);
            consumable.Icon.sprite = Configs.Instance.Items.First(item => item.Id == data.BaitId).Image;
        }
    }
}

public class ConsumableItem {
    [UiRef] public Image Icon;
}