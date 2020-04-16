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

    public void OnEnemyHitPlayer(int damage) {

        float fdamage = Player.Instance.Armor.GetDamage(damage);

        damage = Mathf.RoundToInt(fdamage);
        Player.Instance.DamagePlayer(damage);
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
        if(AbleToAttack)
        {
            PlayerAnimations.Instance.SetAttackSpeed(Player.Instance.GetAttackSpeed());

            NextAttackTime = Time.time + 1f / Player.Instance.GetAttackSpeed();

            PlayerAnimations.Instance.Attack();

            float damageMultipler = EnemyDisplay.EnemyState == EnemyDisplay.State.Stun ? StunnedDamageMultiplier : 1;

            EnemyDisplay.ApplyDamage(Mathf.RoundToInt(Player.Instance.GetLightDamage() * damageMultipler));
            if(Player.Instance.Weapon.IsEnchanted)
                EnemyDisplay.ApplyElementalDamage(Player.Instance.GetElementalDamage(), Player.Instance.Weapon.Enchantment.Config.Type);
            
            Player.Instance.Weapon.SaveData.Taps++;
        }
    }
}
