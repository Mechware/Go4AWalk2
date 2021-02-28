using System;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using G4AW2.Managers;

public class WeaponUI : MonoBehaviour {

    private static readonly string levelNameDAMstringWithElement = @"<b>LVL {0}</b>
<color=#{1}>{2}</color> {3}

DAM {4}+<color=#{1}>{5}</color>";

    private static readonly string levelNameDAMstringWithoutElement = @"<b>LVL {0}</b>
{1}

DAM {2}";


    [Obsolete("Singleton")] public static WeaponUI Instance;
    
    public Button Button1;
    public Button Button2;
    public Button Button3;

    public InventoryItemDisplay WeaponDisplay;
    public TextMeshProUGUI MasteryString;
    public ProgressBarControllerFloat MasteryProgressBar;
    public TextMeshProUGUI LevelNameDAMText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI SellAmountText;

    public ItemViewer ItemViewer;

    public struct ButtonAction {
        public string Title;
        public Action OnClick;
    }

    [SerializeField] private ItemManager _items;
    [SerializeField] private PlayerManager _player;

    public RobustLerperSerialized OpenLerper;
    private enum State { LerpingOpen, LerpingClosed, Open, Closed }
    private State state = State.Closed;

    public void Awake() {
        Instance = this;
    }
    
    public void SetWeaponWithDefaults(WeaponInstance w, Action onfinish=null) {
        SetWeapon(w, new[] {
            new ButtonAction() {Title= w.SaveData.MarkedAsTrash ? "Untrash" : "Trash", OnClick = () => { w.SaveData.MarkedAsTrash = true; onfinish?.Invoke(); } },
            new ButtonAction() {
                Title ="Equip",
                OnClick = () => {
                    if (_player.Weapon == w) return;

                    _items.Add(_player.Weapon);
                    _player.EquipWeapon(w);
                    _items.Remove(w);
                    onfinish?.Invoke();
                }
            },
            new ButtonAction() {
                Title="Close",
                OnClick = () => { onfinish?.Invoke();  }
            },
        });
    }

    public void SetWeapon(WeaponInstance w, ButtonAction[] actions) {

        Open();

        WeaponDisplay.SetDataInstance(w, 1, null, null, false);
        MasteryString.text = "M" + w.Mastery;

        MasteryProgressBar.SetMax(1);
        MasteryProgressBar.SetCurrent(w.RawMastery - Mathf.Floor(w.RawMastery));

        if (w.IsEnchanted) {
            LevelNameDAMText.text = string.Format(
                levelNameDAMstringWithElement,
                w.SaveData.Level,
                ColorUtility.ToHtmlStringRGB(w.Enchantment.Config.Type.DamageColor),
                w.Enchantment.GetPrefix(),
                w.GetName(false, true),
                w.RawDamage,
                w.GetEnchantDamage()
            );
        }
        else {
            LevelNameDAMText.text = string.Format(
                levelNameDAMstringWithoutElement,
                w.SaveData.Level,
                w.GetName(),
                w.RawDamage,
                w.GetEnchantDamage()
            );
        }

        DescriptionText.text = w.Config.Description;
        SellAmountText.text = "Sell: " + w.GetValue();

        if (actions.Length == 0) {
            actions = new [] { new ButtonAction {OnClick = Close, Title = "Close"}};
        }

        Button1.gameObject.SetActive(false);
        Button2.gameObject.SetActive(false);
        Button3.gameObject.SetActive(false);
        Button1.onClick.RemoveAllListeners();
        Button2.onClick.RemoveAllListeners();
        Button3.onClick.RemoveAllListeners();

        if(actions.Length == 1) {
            Button2.gameObject.SetActive(true);
            Button2.GetComponentInChildren<TextMeshProUGUI>().text = actions[0].Title;
            Button2.onClick.AddListener(new UnityAction(actions[0].OnClick));
        }
        if(actions.Length == 2) {
            Button1.gameObject.SetActive(true);
            Button3.gameObject.SetActive(true);
            Button1.GetComponentInChildren<TextMeshProUGUI>().text = actions[0].Title;
            Button3.GetComponentInChildren<TextMeshProUGUI>().text = actions[1].Title;
            Button1.onClick.AddListener(new UnityAction(actions[0].OnClick));
            Button3.onClick.AddListener(new UnityAction(actions[1].OnClick));
        }
        if(actions.Length == 3) {
            Button1.gameObject.SetActive(true);
            Button2.gameObject.SetActive(true);
            Button3.gameObject.SetActive(true);
            Button1.GetComponentInChildren<TextMeshProUGUI>().text = actions[0].Title;
            Button2.GetComponentInChildren<TextMeshProUGUI>().text = actions[1].Title;
            Button3.GetComponentInChildren<TextMeshProUGUI>().text = actions[2].Title;
            Button1.onClick.AddListener(new UnityAction(actions[0].OnClick));
            Button2.onClick.AddListener(new UnityAction(actions[1].OnClick));
            Button3.onClick.AddListener(new UnityAction(actions[2].OnClick));
        }

        Button1.onClick.AddListener(Close);
        Button2.onClick.AddListener(Close);
        Button3.onClick.AddListener(Close);

        SetCompare(_player.Weapon);
    }

    public void Open() {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if(state != State.Closed) {
            OpenLerper.EndLerping(true);
        } else {
            state = State.LerpingOpen;
            OpenLerper.StartLerping(() => {
                state = State.Open;
            });
        }
    }

    void Update() {
        OpenLerper.Update(Time.deltaTime);
    }

    public void Close() {
        state = State.LerpingClosed;
        OpenLerper.StartReverseLerp(() => {
            state = State.Closed;
            gameObject.SetActive(false);
        });
    }

    public TextMeshProUGUI CompareToDescriptionText;
    public ProgressBarControllerFloat CompareToLevelProgress;
    public TextMeshProUGUI CompareToMasteryText;
    public TextMeshProUGUI CompareToSellAmountText;

    public void SetCompare(WeaponInstance w) {
        if(w.IsEnchanted) {
            CompareToDescriptionText.text = string.Format(
                levelNameDAMstringWithElement,
                w.SaveData.Level,
                ColorUtility.ToHtmlStringRGB(w.Enchantment.Config.Type.DamageColor),
                w.Enchantment.GetPrefix(),
                w.GetName(false, true),
                w.RawDamage,
                w.GetEnchantDamage()
            );
        } else {
            CompareToDescriptionText.text = string.Format(
                levelNameDAMstringWithoutElement,
                w.SaveData.Level,
                w.GetName(),
                w.RawDamage,
                w.GetEnchantDamage()
            );
        }

        CompareToLevelProgress.SetMax(1);
        CompareToLevelProgress.SetCurrent(w.RawMastery - Mathf.Floor(w.RawMastery));

        CompareToMasteryText.text = "M" + w.Mastery;

        CompareToSellAmountText.text = "Sell: " + w.GetValue();

        //ItemViewer.Close();
    }

    public void SwapWeapon() {
        ItemViewer.ShowItemsFromInventory<WeaponInstance>("Weapon To Compare", w => { SetCompare((WeaponInstance)w); ItemViewer.Close(); }, false);
        ItemViewer.Add(_player.Weapon, 0, w => { SetCompare((WeaponInstance)w); ItemViewer.Close(); });
    }
}
