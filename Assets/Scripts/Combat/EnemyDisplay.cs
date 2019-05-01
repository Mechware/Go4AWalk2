using G4AW2.Data.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public enum State {
			Idle, BeforeAttack, ExecuteAttack, AfterAttack, Stun, Disabled
		}

	    public PlayerAnimations PlayerAnimations;
	    public ItemDropBubbleManager ItemBubbleManager;


        public FloatReference StunDuration;

		public State EnemyState;
		public EnemyData Enemy;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

	    public FloatReference TimeBetweenHeavyAttacks;
		public FloatReference AttackPrepTime;
		public FloatReference AttackExecuteDuration;
		public IntReference HeavyDamage;

        // Events
		public UnityEventInt OnAttackHit;
		public UnityEventEnemyData OnDeath;
	    public UnityEventIEnumerableLoot OnDropLoot;

	    public DamageNumberSpawner RegularDamageNumberSpawner;
	    public DamageNumberSpawner ElementalDamageNumberSpawner;
        public UnityEventInt OnHit;
		public UnityEventInt OnElementalHit;

        public UnityEvent OnStartWalking;

	    public PlayerFightingLogic FightingLogic;

		private bool isDead = false;
	    private Animator MyAnimator;

	    private RectTransform rt;
	    private RectTransform RectTransform {
	        get {
	            if (rt == null) rt = GetComponent<RectTransform>();
	            return rt;
	        }
	    }

	    void Awake() {
	        MyAnimator = GetComponent<Animator>();
	        rt = GetComponent<RectTransform>();
	    }

        void Start() {
			EnemyState = State.Disabled;

			if (Enemy != null) {
			    SetEnemy(Enemy);
		    }

	    }

		public void SetEnemy( EnemyData data) {
            isDead = false;
			Enemy = data;

			MaxHealth.Value = data.MaxHealth;
			CurrentHealth.Value = MaxHealth;
		    HeavyDamage.Value = data.Damage;
		    TimeBetweenHeavyAttacks.Value = data.TimeBetweenHeavyAttacks;
            AttackPrepTime.Value = data.AttackPrepTime;
            AttackExecuteDuration.Value = data.AttackExecuteTime;

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Death;
			aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
			aoc["BeforeAttack"] = Enemy.BeforeAttack;
			aoc["AttackExecute"] = Enemy.AttackExecute;
			aoc["AfterAttack"] = Enemy.AfterAttack;
			aoc["Idle"] = Enemy.Idle;
            aoc["Walking"] = Enemy.Walking;


            // TODO: Find a better way to do this
            Vector3 pos = transform.localPosition;
            pos.x = -70;
            transform.localPosition = pos;

		    Vector2 r = RectTransform.sizeDelta;
		    r.x = data.SizeOfSprite.x;
		    r.y = data.SizeOfSprite.y;
		    RectTransform.sizeDelta = r;
		}

        public void StartWalking()
        {
            StopAllCoroutines();
            OnStartWalking.Invoke();
            MyAnimator.SetTrigger("Walking");
        }

		public void StartAttacking() {
			StopAllCoroutines();
			StartCoroutine(DoAttack());
		}

		public void Stun() {
			StopAllCoroutines();
		    MyAnimator.SetTrigger("Stun");
        }

        public void UnStun() {
			StartCoroutine(DoAttack());
            MyAnimator.SetTrigger("StunOver");
        }

        #region Attack

		public IEnumerator DoAttack() {
			for (; ; ) {
				EnemyState = State.Idle;
				yield return new WaitForSeconds(TimeBetweenHeavyAttacks);
				EnemyState = State.BeforeAttack;

				if (isDead)
					break;

			    MyAnimator.SetTrigger("AttackStart");

				// Wind up
				yield return new WaitForSeconds(AttackPrepTime);

                if(isDead)
                    break;

                EnemyState = State.ExecuteAttack;

			    MyAnimator.SetTrigger("AttackExecute");

                // Perform the attack
                yield return new WaitForSeconds(AttackExecuteDuration);

                if(isDead)
                    break;

                EnemyState = State.AfterAttack;
			    FightingLogic.OnEnemyHitPlayer(HeavyDamage);
			    if (Enemy.HasElementalDamage) {
			        FightingLogic.OnEnemyHitPlayerElemental(Enemy.ElementalDamage, Enemy.ElementalDamageType);
			    }
                OnAttackHit.Invoke(HeavyDamage);
			    MyAnimator.SetTrigger("AttackEnd");
			}
        }

		public bool AttemptedParry() {
			if(EnemyState == State.ExecuteAttack) {
				EnemyState = State.Stun;
                Stun();
                Timer.StartTimer(this, StunDuration, () =>
                {
                    UnStun();
                });

                return true;
			}
            return false;
		}

	    public Color BaseDamageColor;

		public void ApplyDamage( int amount, bool elemental, EnchantingType type = null ) {
			if (isDead)
				return;

		    if(!elemental) {
		        OnHit.Invoke(amount);
                RegularDamageNumberSpawner.SpawnNumber(amount, BaseDamageColor);
		    } else {
                ElementalDamageNumberSpawner.SpawnNumber(amount, type.DamageColor);
		        OnElementalHit.Invoke(amount);
		    }

            CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
				isDead = true;
                StopAllCoroutines();
				OnDeath.Invoke(Enemy);

			    bool celebrateDone = false;
			    bool bubblesDone = false;

                PlayerAnimations.ResetAttack();
                PlayerAnimations.Celebrate(() => {
                    celebrateDone = true;
                    if(bubblesDone && celebrateDone) {
                        AllDone();
                    }
                });

                MyAnimator.SetTrigger("Death");
			    List<Item> items = Enemy.Drops.GetItems(true);
			    foreach (Item item in items) {
			        if (item is Weapon) {
			            Weapon weapon = item as Weapon;
			            weapon.Level = Enemy.Level;
			        }
			    }

			    ItemBubbleManager.AddItems(items, () => {
			        bubblesDone = true;
			        if (bubblesDone && celebrateDone) {
			            AllDone();
			        }
			    });
                
                OnDropLoot.Invoke(items);
			} else {
                MyAnimator.SetTrigger("Flinch");
            }
        }

	    public UnityEvent CleanUp;
	    public GameObject DeadEnemyPrefab;
	    public Transform DeadEnemyParent;

	    private void AllDone() {
            PlayerAnimations.Spin(() => {
                CleanUp.Invoke();
                var go = Instantiate(DeadEnemyPrefab, DeadEnemyParent);
                DeadEnemy de = go.GetComponent<DeadEnemy>();
                de.SetPosition(RectTransform.anchoredPosition.x, RectTransform.anchoredPosition.y, Enemy);
            });
        }

		#endregion


#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy);
		}
#endif
	}
}

