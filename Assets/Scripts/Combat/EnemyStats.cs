using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyStats")]
public class EnemyStats : ScriptableObject {
    public int Id;
    
    [Header("Stats")]
    public float HealthAtLevel0;
    public float DamageAtLevel0;

    /// <summary>
    /// Minimum attacks per second
    /// </summary>
    public float MinAttackSpeed = 0.5f;
    /// <summary>
    /// Maximum attacks per second
    /// </summary>
    public float MaxAttackSpeed = 4f;

    [Header("Elemental")]
    public bool HasElementalDamage;
    public float ElementalDamageAtLevel0;
    public ElementalType ElementalDamageType;
    public ElementalWeakness ElementalWeaknesses;

    [Header("Misc")]
    public ItemDropper Drops;
    public bool OneAndDoneAttacker = false;

    public EnemyConfig Art;

    public float Weight = 0;
    public int DodgePerk = 0;
}
