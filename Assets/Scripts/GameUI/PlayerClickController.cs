using System;
using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    public MyButton PlayerHeadgear;
    public MyButton PlayerArmor;
    public MyButton PlayerWeapon;

    [Obsolete("Singleton")] public static PlayerClickController Instance;
    public InteractionCoordinator _controller;
    void Awake() {
        Instance = this;
        _controller.OnFightEnter += () => SetEnabled(false);
        _controller.OnEnemyDeathFinished += (e) => SetEnabled(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHeadgear.onClick.AddListener(ChangePlayerHeadgear);  
        
        PlayerArmor.onClick.AddListener(ChangePlayerArmor);

        PlayerWeapon.onClick.AddListener(ChangePlayerWeapon);
    }

    public static void ChangePlayerHeadgear() {
        ItemViewer.Instance.ShowItemsFromInventory<HeadgearInstance>("Equip Headgear", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false); 
    }

    public static void ChangePlayerArmor() {
        ItemViewer.Instance.ShowItemsFromInventory<ArmorInstance>("Equip Armor", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false); 
    }

    public static void ChangePlayerWeapon() {
        ItemViewer.Instance.ShowItemsFromInventory<WeaponInstance>("Equip Weapon", it => {
            EquipItemProcessor.Instance.ProcessItem(it, () => {
                ItemViewer.Instance.Close();
            });
        }, false, false);
    }
    
    
    public void SetEnabled(bool enabled) {
        PlayerWeapon.enabled = enabled;
        PlayerArmor.enabled = enabled;
        PlayerHeadgear.enabled = enabled;
    }
    
}
