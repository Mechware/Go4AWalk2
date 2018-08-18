using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

namespace G4AW2.Data.Combat
{
    public enum Element
    {
        Fire = 0,
        Earth = 1,
        Water = 2,
        Wind = 3
    }

    [CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyData : ScriptableObject
    {
        public int baseHealth;
        public int baseArmor;
	    public int baseAttackSpeed;
	    public int baseDamage;

        public Element element;
        public List<SpellUnlock> spells;

	    public int GetHealth(int level) {
		    return baseHealth;
	    }

	    public int GetArmor( int level ) {
		    return baseArmor;
	    }

	    public int GetAttackSpeed( int level ) {
		    return baseAttackSpeed;
	    }

	    public int GetDamage(int level) {
		    return baseDamage;
	    }
	}


    [Serializable]
    public class SpellUnlock
    {
        public SpellData spell;
        public int unlockLevel;
    }
}


