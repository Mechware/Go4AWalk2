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

        public FloatReference TimeToCounter;
		public FloatReference TimeBetweenLightAttacks;
	    public FloatReference TimeBetweenHeavyAttacks;
	    public IntReference HeavyDamage;

        // Events
        public UnityEventSwipe OnSwipe;
        public UnityEvent OnSwipeBreak;
        public UnityEventFloat OnSwipeHit;
	    public UnityEventInt OnLightAttack;
	    public UnityEvent OnDeath;
	    public UnityEventInt OnHit;

		private bool isDead = false;

	    void Start() {
		    if (Enemy != null) {
			    SetEnemy(Enemy, Level);
		    }
	    }

	    public IEnumerator Attack() {
		    for (;;) {
				yield return new WaitForSeconds(1f / TimeBetweenLightAttacks);
				if(isDead)
					break;
                OnLightAttack.Invoke(HeavyDamage);
			}
		}

	    public IEnumerator DoSwipingAttack() {
		    for (;;) {
			    yield return new WaitForSeconds(1f / TimeBetweenHeavyAttacks);
			    if (isDead)
				    break;
				Swipe swipe = Enemy.Swipes.GetSwipe(Level);
			    OnSwipe.Invoke(swipe);
                yield return new WaitForSeconds(swipe.GetEntireSwipeTime());
            }
        }

	    public void SwipeCompleted(Swipe s) {
            OnSwipeHit.Invoke(HeavyDamage);
	    }

	    public void SetEnemy(EnemyData data, int level) {
		    Enemy = data;
		    Level.Value = level;

		    MaxHealth.Value = data.GetHealth(level);
		    CurrentHealth.Value = MaxHealth;
		    HeavyDamage.Value = data.GetHeavyDamage(level);
		    TimeBetweenLightAttacks.Value = data.GetTimeBetweenLightAttacks(level);
			TimeBetweenHeavyAttacks.Value = data.GetTimeBetweenHeavyAttacks(level);

			StopAllCoroutines();
		    //StartCoroutine(Attack());
		    StartCoroutine(DoSwipingAttack());

		    AnimatorOverrideController aoc = (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;
		    aoc["Death"] = Enemy.Death;
		    aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
            aoc["LightAttack"] = Enemy.LightAttack;
            aoc["BeforeSwipeAttack"] = Enemy.BeforeSwipeAttack;
		    aoc["SwipeAttack"] = Enemy.SwipeAttack;
		    aoc["Idle"] = Enemy.Idle;
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

