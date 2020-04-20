using G4AW2.Data.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

	    public static EnemyDisplay Instance;
	    
		public enum State {
			Idle, BeforeAttack, ExecuteAttack, AfterAttack, Stun, Disabled, Dead
		}

        [Header("Misc References")]
	    public DamageNumberSpawner RegularDamageNumberSpawner;
	    public DamageNumberSpawner ElementalDamageNumberSpawner;
	    public TextMeshProUGUI EnemyInfo;
	    
        [Header("Settings")]
        public FloatReference StunDuration;
	    public Color BaseDamageColor;
	    public Color ParryColor;
	    public Color BlockColor;

        [Header("Data")]
		public State EnemyState;
		public EnemyInstance Enemy;

        [Header("Readable Data")]
		public double MaxHealth;
	    public double CurrentHealth;

        private Animator MyAnimator;

	    private Image im;
	    private Image Image {
	        get {
	            if(im == null)
	                im = GetComponent<Image>();
	            return im;
	        }
	    }

	    private RectTransform rt;
	    public RectTransform RectTransform {
	        get {
	            if (rt == null) rt = GetComponent<RectTransform>();
	            return rt;
	        }
	    }

	    void Awake() {
		    Instance = this;
            MyAnimator = GetComponent<Animator>();
	        rt = GetComponent<RectTransform>();
			EnemyState = State.Disabled;
			gameObject.SetActive(false);
        }

		public void SetEnemy( EnemyInstance instance) {
			EnemyState = State.Idle;
			Enemy = instance;
            
			MaxHealth = instance.MaxHealth;
			CurrentHealth = MaxHealth;

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Config.Art.Death;
			aoc["Dead"] = Enemy.Config.Art.Dead;
			aoc["Flinch"] = Enemy.Config.Art.Flinch;
			aoc["BeforeAttack"] = Enemy.Config.Art.BeforeAttack;
			aoc["AttackExecute"] = Enemy.Config.Art.AttackExecute;
			aoc["AfterAttack"] = Enemy.Config.Art.AfterAttack;
			aoc["Idle"] = Enemy.Config.Art.Idle;
            aoc["Walking"] = Enemy.Config.Art.Walking;


            // TODO: Find a better way to do this
            Vector3 pos = transform.localPosition;
            pos.x = 70;
            transform.localPosition = pos;

		    Vector2 r = RectTransform.sizeDelta;
		    r.x = Enemy.Config.Art.SizeOfSprite.x;
		    r.y = Enemy.Config.Art.SizeOfSprite.y;
		    RectTransform.sizeDelta = r;

		    Image.color = Color.white;

		    EnemyInfo.text = $"{Enemy.Config.Art.DisplayName}\nLevel {Enemy.SaveData.Level}";
		}

	    public void StartWalkingAnimation() {
		    StopAllCoroutines();
		    MyAnimator.SetTrigger("Walking");
        }

	    public void StopWalking() {
		    MyAnimator.SetTrigger("DoneWalking");
	    }

		public void StartAttacking() {
			StopAllCoroutines();
			StartCoroutine(DoAttack(true));
		}

		public void Stun() {
			StopAllCoroutines();
		    MyAnimator.SetTrigger("Stun");
		    Image.color = Color.white;
			EnemyState = State.Stun;
		    Timer.StartTimer(StunDuration, UnStun, this);
        }

        public void UnStun() {
			StartCoroutine(DoAttack());
            MyAnimator.SetTrigger("StunOver");
        }

		public IEnumerator DoAttack(bool first = false) {
			for (; ; ) {
				EnemyState = State.Idle;
				float attackSpeed = Enemy.GetRandomAttackSpeed();
                yield return new WaitForSeconds(1f/attackSpeed);
                first = false;
			    if (EnemyState == State.Dead) break;

			    MyAnimator.SetTrigger("AttackStart");
			    
			    yield return new WaitForSeconds(Enemy.Config.Art.AttackDurationUntilDamage);
			    
			    PlayerFightingLogic.Instance.OnEnemyHitPlayer(attackSpeed, Enemy.Damage);
			    if (Enemy.Config.HasElementalDamage) {
				    PlayerFightingLogic.Instance.OnEnemyHitPlayerElemental(attackSpeed, Enemy.ElementalDamage, Enemy.Config.ElementalDamageType);
			    }

                if (Enemy.Config.OneAndDoneAttacker) {
	                Die(true);
			        break;
			    }

			    MyAnimator.SetTrigger("AttackEnd");
            }
        }

		public void ApplyDamage( double amount) {
		    if(EnemyState == State.Dead)
		        return;


            CurrentHealth -= amount;
			if (CurrentHealth <= 0) {
			    Die(false);
			} else {
                MyAnimator.SetTrigger("Flinch");
            }
        }

	    public void ApplyElementalDamage(double amount, ElementalType type) {
	        if(EnemyState == State.Dead)
	            return;

            amount = amount * Enemy.GetElementalWeakness(type);

            CurrentHealth -= amount;
	        if(CurrentHealth <= 0) {
	            Die(false);
	        } else {
	            MyAnimator.SetTrigger("Flinch");
	        }
	    }

        private void Die(bool suicide) {
	        EnemyState = State.Dead;
            Image.color = Color.white;
	        StopAllCoroutines();
	        InteractionController.Instance.EnemyDeath(Enemy, suicide);

	        MyAnimator.SetTrigger("Death");
        }

#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy);
		}
#endif
	}
}

