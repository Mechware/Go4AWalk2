using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using G4AW2.Followers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : MonoBehaviour {

    public IntReference GoldAmount;

    public GameObject IconWithTextPrefab;
    public GameObject BuyingScrollPanelContent;
    public GameObject SellingScrollPanelContent;

    public TextMeshProUGUI TrashButtonText;

    public UnityEvent OnFinish;

    private ShopFollowerInstance shopKeep;

    private List<GameObject> buyingItems = new List<GameObject>();
    private List<GameObject> sellingItems = new List<GameObject>();

    private Action actionOnSendAway;

    public void OpenShop(ShopFollowerInstance shopKeep, Action actionOnSendAway) {
        this.actionOnSendAway = actionOnSendAway;
        this.shopKeep = shopKeep;
        gameObject.SetActive(true);
        GetComponent<RobustLerper>().StartLerping();

        RefreshBuyingList();
        SetSellingTab();
    }

    public void Finish() {
        actionOnSendAway?.Invoke();
        GetComponent<RobustLerper>().StartReverseLerp();
        OnFinish.Invoke();
    }

    public void SetSellingTab() {
        RefreshSellingList();
        UpdateTrashButtonText();
    }

    public void UpdateTrashButtonText() {
        TrashButtonText.text = $"Sell Trash + Shards ({GetTrashSum()} gold)";
    }

    private int GetTrashSum() {
        int sum = 0;
        foreach(ItemInstance i in Inventory.Instance) {

            if(i.SaveData.MarkedAsTrash || i.Config.SellWithTrash) {
                sum += Mathf.RoundToInt(i.GetValue() * (shopKeep.Config.SellingPriceMultiplier));
            }
        }
        return sum;
    }

    public void SellTrash() {
        List<ItemInstance> toRemove = new List<ItemInstance>();
        foreach(var i in Inventory.Instance) {
            if(i.SaveData.MarkedAsTrash || i.Config.SellWithTrash) {
                toRemove.Add(i);
            }
        }

        GoldAmount.Value += GetTrashSum();
        toRemove.ForEach(i => Inventory.Instance.Remove(i));

        SetSellingTab();
    }

    #region Buying
    public void RefreshBuyingList() {
        buyingItems.ForEach(Destroy);
        buyingItems.Clear();

        foreach(var item in shopKeep.Items) {
            var go = Instantiate(IconWithTextPrefab, BuyingScrollPanelContent.transform);
            var itemDisplay = go.GetComponent<IconWithTextController>();
            SetDataBuying(itemDisplay, item);
            buyingItems.Add(go);
        }
    }

    private int GetBuyingPrice(ItemInstance i) {
        return Mathf.Max(Mathf.RoundToInt(i.GetValue() * shopKeep.Config.BuyingPriceMultiplier), 1);
    }

    private void SetDataBuying(IconWithTextController itc, ItemInstance iid) {
        int price = GetBuyingPrice(iid);

        string text = $"{iid.GetName()}\n{price} gold each";
        itc.SetDataInstance(iid, 1, text, () => ItemClickedBuying(iid));
    }

    private void ItemClickedBuying(ItemInstance it) {
        int price = GetBuyingPrice(it);

        if (GoldAmount.Value < price) {
            PopUp.SetPopUp("Not Enough Gold.", new[] {":(", "):"}, new Action[] {() => { }, () => { }});
            return;
        }

        string title =
            $"Would you like to buy a {it.GetName()} for {price} gold?\n{it.GetDescription()}";

        PopUp.SetPopUp(title, new [] { "Buy", "Cancel" },
            new Action[] {
                () => {
                    GoldAmount.Value -= price;
                    Inventory.Instance.Add(it);
                    shopKeep.Items.Remove(it);
                    RefreshBuyingList();
                },
                () => {
                    RefreshBuyingList();
                }
            });
    }


    #endregion

    #region Selling
    public void RefreshSellingList() {
        sellingItems.ForEach(Destroy);
        sellingItems.Clear();

        foreach(var item in Inventory.Instance) {
            var go = Instantiate(IconWithTextPrefab, SellingScrollPanelContent.transform);
            var itemDisplay = go.GetComponent<IconWithTextController>();
            SetDataSelling(itemDisplay, item);
            sellingItems.Add(go);
        }
    }

    private int GetSellingPrice(ItemInstance i) {
        return Mathf.Max(Mathf.RoundToInt(i.GetValue() * shopKeep.Config.SellingPriceMultiplier), 1);
    }

    private void SetDataSelling(IconWithTextController itc, ItemInstance iid) {
        string text = iid.GetName() + "\n" + GetSellingPrice(iid) + " gold each";
        itc.SetDataInstance(iid, 1, text, () => ItemClickedSelling(iid));
    }

    private void ItemClickedSelling(ItemInstance it) {

        int price = GetSellingPrice(it);
        
        string title = string.Format($"Confirm sale of {it.GetName()} for {price} gold?\n{it.GetDescription()}");

        PopUp.SetPopUp(title, new string[] { "Sell", "Cancel" },
           new Action[] {
            () => {
                GoldAmount.Value += price;
                Inventory.Instance.Remove(it);
                RefreshSellingList();
                },
            () => {
                
                },
            }
        );

        
    }

    #endregion
}
