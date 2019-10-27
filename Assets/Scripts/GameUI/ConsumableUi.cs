using Items;
using Scripts.DoubleJump;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUi : MonoBehaviour
{
    public RectTransform RegularConsumableParent;
    public RectTransform RegularConsumablePrefab;

    public static ConsumableUi Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        while(RegularConsumableParent.childCount > 0)
            Destroy(RegularConsumableParent.GetChild(0));

        foreach(var data in ConsumableManager.Instance.SaveData) {
            var it = Instantiate(RegularConsumablePrefab, RegularConsumableParent);
            var consumable = UiRef.Init<ConsumableItem>(it.gameObject);
            consumable.Icon.sprite = DataManager.Instance.AllItems.First(item => item.ID == data.Id).Image;
        }

        foreach(var data in BaitBuffController.Instance.SaveData) {
            var it = Instantiate(RegularConsumablePrefab, RegularConsumableParent);
            var consumable = UiRef.Init<ConsumableItem>(it.gameObject);
            consumable.Icon.sprite = DataManager.Instance.AllItems.First(item => item.ID == data.BaitId).Image;
        }
    }
}

public class ConsumableItem {
    [UiRef] public Image Icon;
}