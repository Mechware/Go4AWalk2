using System;
using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFightingLogic : MonoBehaviour {

    public static PlayerFightingLogic Instance;
    
	public EnemyDisplay EnemyDisplay;

    public DamageNumberSpawner PlayerDamageNumberSpawner;
    public Color DamageColor;

    private bool AbleToAttack => Time.time > NextAttackTime;
    private float NextAttackTime = 0;

    private void Awake() {
        Instance = this;
    }

    public void Initialize() {
        PlayerAnimations.Instance.SetAttackSpeed(1);
    }
    
    public void OnEnemyHitPlayer(double damage) {

        double fdamage = Player.Instance.Armor.GetDamage(damage);
        Player.Instance.DamagePlayer(fdamage);
	    PlayerDamageNumberSpawner.SpawnNumber(damage, DamageColor);
    }

    public void OnEnemyHitPlayerElemental(int damage, ElementalType damageType) {

        float mod = 1;
        if (Player.Instance.Armor.Config.ElementalWeakness != null)
            mod = Player.Instance.Armor.Config.ElementalWeakness[damageType];
        float fdamage = damage * mod;

        damage = Mathf.RoundToInt(fdamage);
        Player.Instance.DamagePlayer(damage);
        PlayerDamageNumberSpawner.SpawnNumber(damage, damageType.DamageColor);
    }

    public float StunnedDamageMultiplier = 1.15f;

	public void PlayerAttemptToHitEnemy() {
        if (!AbleToAttack) return;
        float attkSpeed = Player.Instance.GetAttackSpeed();

        NextAttackTime = Time.time + 1f / attkSpeed;


        float damageMultipler = EnemyDisplay.EnemyState == EnemyDisplay.State.Stun ? StunnedDamageMultiplier : 1;
        double dmg = Math.Round(Player.Instance.GetDamage() * damageMultipler);
        
        double elemDmg = Player.Instance.GetElementalDamage();
        ElementalType type = Player.Instance.Weapon.Enchantment?.Config.Type;
        
        PlayerAnimations.Instance.Attack(dmg, elemDmg, type, _OnDone);


        void _OnDone() {
            
            EnemyDisplay.ApplyDamage(dmg);

            if(type != null)
                EnemyDisplay.ApplyElementalDamage(elemDmg, type);

            
            Player.Instance.Weapon.SaveData.Taps++;
            if (!SaveGame.SaveData.IdsToNumberOfTaps.ContainsKey(Player.Instance.Weapon.Config.Id))
                SaveGame.SaveData.IdsToNumberOfTaps[Player.Instance.Weapon.Config.Id] = 0;
        
            SaveGame.SaveData.IdsToNumberOfTaps[Player.Instance.Weapon.Config.Id] += 1;
        }
    }
}
