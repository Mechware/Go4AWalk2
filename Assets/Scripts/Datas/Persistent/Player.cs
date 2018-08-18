using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Utils;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player")]
public class Player : ScriptableObject {

	[SerializeField] private ObservedValue<int> Health;
	[SerializeField] private int Damage;


	public void LightAttack(EnemyDisplay e) {
		e.CurrentEnemy.ApplyDamage(Damage);
	}

	public void HeavyAttack(EnemyDisplay e, Vector3[] points) {
		e.CurrentEnemy.ApplyDamage(Damage);
	}
}
