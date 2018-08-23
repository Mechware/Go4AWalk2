using G4AW2.Data.Combat;
using System;
using System.Collections;
using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
    public class EnemyDisplay : MonoBehaviour {

		public EnemyData Enemy;
	    public IntReference Level;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

	    public IntReference CurrentCrit;

	    public FloatReference AttackSpeed;
	    public IntReference Damage;

		// Events
	    public UnityEventInt OnAttack;
	    public UnityEvent OnDeath;
	    public UnityEventSwipe OnSwipe;

		private bool isDead = false;

	    void Start() {
		    if (Enemy != null) {
			    SetEnemy(Enemy, Level);
		    }
	    }

	    public IEnumerator Attack() {
		    for (;;) {
				yield return new WaitForSeconds(1f / AttackSpeed);
			    OnAttack.Invoke(Damage);
			}
		}

	    public void DoSwipingAttack() {
		    Swipe swipe = Enemy.Swipes.GetSwipe(Level);
		    OnSwipe.Invoke(swipe);
	    }

	    public void SetEnemy(EnemyData data, int level) {
		    Enemy = data;
		    Level.Value = level;

		    MaxHealth.Value = data.GetHealth(level);
		    CurrentHealth.Value = MaxHealth;
		    Damage.Value = data.GetDamage(level);
		    AttackSpeed.Value = data.GetAttackSpeed(level);

		    CurrentCrit.Value = 0;
			StopCoroutine("Attack");
		    StartCoroutine(Attack());
	    }

	    public void ApplyDamage(int amount) {
		    if (isDead) return;
		    CurrentHealth.Value -= amount;
		    if (CurrentHealth.Value <= 0) {
				print("Dying");
			    isDead = true;
				OnDeath.Invoke();
		    }
	    }

#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy, Level);
		}
#endif
	}
}

