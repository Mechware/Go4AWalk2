using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using G4AW2.Questing;
using Items;
using UnityEngine;

public class Game : MonoBehaviour {
    public WeaponConfig StartWeapon;
    public ArmorConfig StartArmor;
    public QuestConfig StartQuest;
    
    public void Start() {
        bool newGame = SaveGame.Instance.Load();

        if (newGame) {
            Player.Instance.MaxHealth = 100;
            Player.Instance.Health = Player.Instance.MaxHealth;
            
            Player.Instance.Weapon = new WeaponInstance(StartWeapon, 1);
            Player.Instance.Weapon.SaveData.Random = 30;

            Player.Instance.Armor = new ArmorInstance(StartArmor, 1);
            Player.Instance.Armor.SaveData.Random = 30;
            
            QuestManager.Instance.CurrentQuest = new QuestInstance(StartQuest);
        }
        
        //TODO: Load int/float variables
        
        //AchievementManager.Instance.InitAchievements();
        QuestManager.Instance.Initialize();
        Player.Instance.Initialize();
        MainUI.Instance.Initialize();
        ConsumableManager.Instance.Initialize();
        PlayerFightingLogic.Instance.Initialize();
        InteractionController.Instance.InitializeAndStart();
    }

    void Update() {
        MainUI.Instance.MyUpdate();
        QuestingStatWatcher.Instance.MyUpdate();
        ConsumableManager.Instance.MyUpdate();
        QuestManager.Instance.MyUpdate();
    }
}
