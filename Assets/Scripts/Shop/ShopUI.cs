using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour {

    public RuntimeSetFollowerData Followers;
    public Inventory Inventory;
    public IntReference GoldAmount;

    public GameObject IconWithTextPrefab;
    public GameObject BuyingScrollPanelContent;
    public GameObject SellingScrollPanelContent;

    public TextMeshProUGUI TrashButtonText;

    private ShopFollower shopKeep;

    private List<GameObject> buyingItems = new List<GameObject>();
    private List<GameObject> sellingItems = new List<GameObject>();

    public void OpenShop(ShopFollower shopKeep) {
        this.shopKeep = shopKeep;
        gameObject.SetActive(true);
        GetComponent<RobustLerper>().StartLerping();

        RefreshBuyingList();
        SetSellingTab();
    }

    public void Finish() {
        Followers.Remove(shopKeep);
        GetComponent<RobustLerper>().StartReverseLerp();
    }

    public void SetSellingTab() {
        RefreshSellingList();
        UpdateTrashButtonText();
    }

    public void UpdateTrashButtonText() {
        int sum = 0;
        foreach (var i in Inventory) {
            if (i.Item is Weapon && ((Weapon)i.Item).MarkedAsTrash) {
                sum += Mathf.RoundToInt(i.Item.Value * i.Amount * (shopKeep.SellingPriceMultiplier));
            }
            if (i.Item is Armor && ((Armor) i.Item).IsMarkedTrash) {
                sum += Mathf.RoundToInt(i.Item.Value * i.Amount * (shopKeep.SellingPriceMultiplier));
            }
        }
        TrashButtonText.text = $"Sell Trash ({sum} gold)";
    }

    public void SellTrash() {
        int sum = 0;
        List<InventoryEntry> toRemove = new List<InventoryEntry>();
        foreach(var i in Inventory) {
            if(i.Item is Weapon && ((Weapon) i.Item).MarkedAsTrash) {
                sum += Mathf.RoundToInt(i.Item.Value * i.Amount * (shopKeep.SellingPriceMultiplier));
                toRemove.Add(i);
            }
            if(i.Item is Armor && ((Armor) i.Item).IsMarkedTrash) {
                sum += Mathf.RoundToInt(i.Item.Value * i.Amount * (shopKeep.SellingPriceMultiplier));
                toRemove.Add(i);
            }
        }

        GoldAmount.Value += sum;
        toRemove.ForEach(i => Inventory.Remove(i));

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

    private void SetDataBuying(IconWithTextController itc, InventoryEntry iid) {
        int price = Mathf.RoundToInt(iid.Item.Value * shopKeep.BuyingPriceMultiplier);

        string text = $"{iid.Item.Name}\n{price} gold each";
        itc.SetData(iid.Item, iid.Amount, text, () => ItemClickedBuying(iid));
    }

    private void ItemClickedBuying(InventoryEntry it) {
        int price = Mathf.RoundToInt(it.Item.Value * shopKeep.BuyingPriceMultiplier);

        string title = string.Format("Would you like to buy a {0} for {1} gold?\nDescription:{2}",
            it.Item.Name,
            price,
            it.Item.GetDescription());

        PopUp.SetPopUp(title, new string[] { "Yes", "No" },
            new Action[] {
                () => {
                    if(GoldAmount.Value < price) {
                        PopUp.SetPopUp("Not enough gold :(", new string[] {"ok"}, new Action[] {()=> { } });
                        return;
                    }

                    GoldAmount.Value -= Mathf.RoundToInt(it.Item.Value * shopKeep.BuyingPriceMultiplier);
                    Inventory.Add(it.Item, 1);
                    it.Amount -= 1;
                    if(it.Amount == 0) shopKeep.Items.Remove(it);
                    PopUp.SetPopUp("Success!", new[] {"Ok"}, new Action[] {
                        () => {
                            if (it.Amount > 0) {
                                ItemClickedBuying(it);
                            }
                            else {
                                RefreshBuyingList();
                            }
                        }
                    });

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

        foreach(var item in Inventory) {
            var go = Instantiate(IconWithTextPrefab, SellingScrollPanelContent.transform);
            var itemDisplay = go.GetComponent<IconWithTextController>();
            SetDataSelling(itemDisplay, item);
            sellingItems.Add(go);
        }
    }

    private void SetDataSelling(IconWithTextController itc, InventoryEntry iid) {
        string text = iid.Item.Name + "\n" + Mathf.RoundToInt(iid.Item.Value * shopKeep.SellingPriceMultiplier) + " gold each";
        itc.SetData(iid.Item, iid.Amount, text, () => ItemClickedSelling(iid));
    }

    private void ItemClickedSelling(InventoryEntry it) {
        string title = string.Format("Would you like to sell a {0} for {1} gold?\nDescription:{2}",
            it.Item.Name,
            Mathf.RoundToInt(it.Item.Value * shopKeep.SellingPriceMultiplier),
            it.Item.GetDescription());

        PopUp.SetPopUp(title, new string[] { "Yes", "No" },
            new Action[] {
                () => {
                    GoldAmount.Value += Mathf.RoundToInt(it.Item.Value * shopKeep.SellingPriceMultiplier);
                    Inventory.Remove(it.Item, 1);

                    PopUp.SetPopUp("Success!", new[] {"Ok"}, new Action[] {
                        () => {
                            if (it.Amount > 0) {
                                ItemClickedSelling(it);
                            }
                            else {
                                RefreshSellingList();
                            }
                        }
                    });

                },
                () => {
                    RefreshSellingList();
                }
            });
    }


    #endregion
}
