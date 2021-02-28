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

		[SerializeField] private float _stunnedDamageMultiplier = 1.15f;

		public Action<int, ElementalType> OnDamageTaken;
		public Action<int, ElementalType> OnAttack;
		public Action<EnemyInstance> OnDeath;


		[Header("Misc References")]
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

		public void OnHit(float damage, ElementalType type)
        {
			if (EnemyState == State.Dead)
				return;

			float mod = 1;
			if(type == null)
            {
				mod = EnemyState == State.Stun ? _stunnedDamageMultiplier : 1;
			} 
			else
            {
				mod = Enemy.GetElementalWeakness(type);
			}

			int amount = Mathf.RoundToInt(damage * mod);

			OnDamageTaken?.Invoke(amount, type);

			CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0)
			{
				Die(false);
			}
			else
			{
				MyAnimator.SetTrigger("Flinch");
			}
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
				OnAttack?.Invoke(Enemy.Damage, null);
			    if (Enemy.Config.HasElementalDamage) {
					OnAttack?.Invoke(Enemy.ElementalDamage, Enemy.Config.ElementalDamageType);
			    }

                if (Enemy.Config.OneAndDoneAttacker) {
	                Die(true);
			        break;
			    }

			    MyAnimator.SetTrigger("AttackEnd");
            }
        }

		public void Stop()
        {
			StopAllCoroutines();
        }

        private void Die(bool suicide) {
	        EnemyState = State.Dead;
            Image.color = Color.white;
	        StopAllCoroutines();
			Enemy.Suicide = suicide;
			OnDeath?.Invoke(Enemy);

	        MyAnimator.SetTrigger("Death");
        }

		public void DebugKill()
        {
			OnHit(999999999999, null);
        }

#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy);
		}
#endif
	}
}

