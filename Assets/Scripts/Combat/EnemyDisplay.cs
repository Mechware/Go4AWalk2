using G4AW2.Data.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Events;
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
		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

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
            
			MaxHealth.Value = instance.MaxHealth;
			CurrentHealth.Value = MaxHealth;

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Config.Death;
			aoc["Dead"] = Enemy.Config.Dead;
			aoc["Flinch"] = Enemy.Config.Flinch;
			aoc["BeforeAttack"] = Enemy.Config.BeforeAttack;
			aoc["AttackExecute"] = Enemy.Config.AttackExecute;
			aoc["AfterAttack"] = Enemy.Config.AfterAttack;
			aoc["Idle"] = Enemy.Config.Idle;
            aoc["Walking"] = Enemy.Config.Walking;


            // TODO: Find a better way to do this
            Vector3 pos = transform.localPosition;
            pos.x = -70;
            transform.localPosition = pos;

		    Vector2 r = RectTransform.sizeDelta;
		    r.x = Enemy.Config.SizeOfSprite.x;
		    r.y = Enemy.Config.SizeOfSprite.y;
		    RectTransform.sizeDelta = r;

		    Image.color = Color.white;

		    EnemyInfo.text = $"{Enemy.Config.DisplayName}\nLevel {Enemy.SaveData.Level}";
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
                yield return new WaitForSeconds(first ? Enemy.Config.TimeBetweenAttacks / 4 : Enemy.Config.TimeBetweenAttacks);
                first = false;
			    if (EnemyState == State.Dead) break;

				EnemyState = State.BeforeAttack;
			    MyAnimator.SetTrigger("AttackStart");
			    if(SaveGame.SaveData.ShowParryAndBlockColors) Image.color = BlockColor;

				// Wind up
				yield return new WaitForSeconds(Enemy.Config.AttackPrepTime);
			    if(EnemyState == State.Dead)
			        break;


                EnemyState = State.ExecuteAttack;
			    MyAnimator.SetTrigger("AttackExecute");
			    if(SaveGame.SaveData.ShowParryAndBlockColors)
			        Image.color = ParryColor;

                // Perform the attack
                yield return new WaitForSeconds(Enemy.Config.AttackExecuteTime);
			    if(SaveGame.SaveData.ShowParryAndBlockColors)
			        Image.color = Color.white;
			    if(EnemyState == State.Dead)
			        break;

                EnemyState = State.AfterAttack;
			    PlayerFightingLogic.Instance.OnEnemyHitPlayer(Enemy.Damage);
			    if (Enemy.Config.HasElementalDamage) {
				    PlayerFightingLogic.Instance.OnEnemyHitPlayerElemental(Enemy.ElementalDamage, Enemy.Config.ElementalDamageType);
			    }

                if (Enemy.Config.OneAndDoneAttacker) {
	                Die(true);
			        break;
			    }

			    MyAnimator.SetTrigger("AttackEnd");
            }
        }

		public void ApplyDamage( int amount) {
		    if(EnemyState == State.Dead)
		        return;

            RegularDamageNumberSpawner.SpawnNumber(amount, BaseDamageColor);

            CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
			    Die(false);
			} else {
                MyAnimator.SetTrigger("Flinch");
            }
        }

	    public void ApplyElementalDamage(int amount, ElementalType type) {
	        if(EnemyState == State.Dead)
	            return;

            amount = Mathf.RoundToInt(amount * Enemy.GetElementalWeakness(type));
	        ElementalDamageNumberSpawner.SpawnNumber(amount, type.DamageColor);

            CurrentHealth.Value -= amount;
	        if(CurrentHealth.Value <= 0) {
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

