using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat.Swiping;
using UnityEditor.Sprites;
using UnityEngine;

namespace G4AW2.Data.Combat
{
    [CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyData : FollowerData
    {
        public int BaseHealth;
        public int BaseArmor;
	    [Tooltip("Attacks per second")] public float BaseAttackSpeed;
	    public int BaseDamage;
	    public SwipeSet Swipes;

	    public int GetHealth(int level) {
		    return BaseHealth;
	    }

	    public int GetArmor( int level ) {
		    return BaseArmor;
	    }

	    public float GetAttackSpeed( int level ) {
		    return BaseAttackSpeed;
	    }

	    public int GetDamage(int level) {
		    return BaseDamage;
	    }
	}
}


