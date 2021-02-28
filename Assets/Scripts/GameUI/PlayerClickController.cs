using System;
using UnityEngine;

public class PlayerClickController : MonoBehaviour {
    public MyButton PlayerHeadgear;
    public MyButton PlayerArmor;
    public MyButton PlayerWeapon;

    [SerializeField] private InteractionCoordinator _controller;
    [SerializeField] private ItemViewer _itemViewer;
    [SerializeField] private EquipItemProcessor _equipItemProcessor;


    void Awake() {
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

    public void ChangePlayerHeadgear() {
        _itemViewer.ShowItemsFromInventory<HeadgearInstance>("Equip Headgear", it => {
            _equipItemProcessor.ProcessItem(it, () => {
                _itemViewer.Close();
            });
        }, false); 
    }

    public void ChangePlayerArmor() {
        _itemViewer.ShowItemsFromInventory<ArmorInstance>("Equip Armor", it => {
            _equipItemProcessor.ProcessItem(it, () => {
                _itemViewer.Close();
            });
        }, false); 
    }

    public void ChangePlayerWeapon() {
        _itemViewer.ShowItemsFromInventory<WeaponInstance>("Equip Weapon", it => {
            _equipItemProcessor.ProcessItem(it, () => {
                _itemViewer.Close();
            });
        }, false, false);
    }
    
    
    public void SetEnabled(bool enabled) {
        PlayerWeapon.enabled = enabled;
        PlayerArmor.enabled = enabled;
        PlayerHeadgear.enabled = enabled;
    }
    
}
