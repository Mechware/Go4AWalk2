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
    public class EnemyData : FollowerData
    {
        public int BaseHealth;
        public int BaseArmor;
	    public int BaseAttackSpeed;
	    public int BaseDamage;

	    public int GetHealth(int level) {
		    return BaseHealth;
	    }

	    public int GetArmor( int level ) {
		    return BaseArmor;
	    }

	    public int GetAttackSpeed( int level ) {
		    return BaseAttackSpeed;
	    }

	    public int GetDamage(int level) {
		    return BaseDamage;
	    }
	}
}


