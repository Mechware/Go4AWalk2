using G4AW2.Data.Combat;
using System;
using System.Collections;
using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public EnemyData Enemy;
	    public IntReference Level;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

	    public IntReference CurrentCrit;

		public FloatReference AttackSpeed;
	    public FloatReference HeavyAttackSpeed;
	    public IntReference Damage;

		// Events
	    public UnityEventInt OnAttack;
	    public UnityEvent OnDeath;
	    public UnityEventSwipe OnSwipe;
	    public UnityEventInt OnHit;

		private bool isDead = false;

	    void Start() {
		    if (Enemy != null) {
			    SetEnemy(Enemy, Level);
		    }
	    }

	    public IEnumerator Attack() {
		    for (;;) {
				yield return new WaitForSeconds(1f / AttackSpeed);
				if(isDead)
					break;
			    OnAttack.Invoke(Damage);
			}
		}

	    public IEnumerator DoSwipingAttack() {
		    for (;;) {
			    yield return new WaitForSeconds(1f / HeavyAttackSpeed);
			    if (isDead)
				    break;
				Swipe swipe = Enemy.Swipes.GetSwipe(Level);
			    OnSwipe.Invoke(swipe);
			}
	    }

	    public void SwipeCompleted(Swipe s) {
		    OnAttack.Invoke(Damage * CurrentCrit);
	    }

	    public void SwipeBroken(Swipe s) {
		    // ?
	    }

	    public void SetEnemy(EnemyData data, int level) {
		    Enemy = data;
		    Level.Value = level;

		    MaxHealth.Value = data.GetHealth(level);
		    CurrentHealth.Value = MaxHealth;
		    Damage.Value = data.GetDamage(level);
		    AttackSpeed.Value = data.GetAttackSpeed(level);
			HeavyAttackSpeed.Value = data.GetHeavyAttackSpeed(level);

		    CurrentCrit.Value = 0;
			StopAllCoroutines();
		    StartCoroutine(Attack());
		    StartCoroutine(DoSwipingAttack());

		    AnimatorOverrideController aoc = (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;
		    aoc["Death"] = Enemy.Death;
		    aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
		    aoc["HeavyAttack"] = Enemy.HeavyAttack;
		    aoc["Idle"] = Enemy.Idle;
		    aoc["LightAttack"] = Enemy.Attack;
		}

	    public void ApplyDamage(int amount) {
		    if (isDead) return;

			CurrentHealth.Value -= amount;
		    if (CurrentHealth.Value <= 0) {
			    isDead = true;
			    OnDeath.Invoke();
		    }
		    else {
				OnHit.Invoke(amount);
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

