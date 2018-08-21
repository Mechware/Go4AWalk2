using G4AW2.Data.Combat;
using System;
using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
    public class EnemyDisplay : MonoBehaviour {

		public EnemyInstance CurrentEnemy;

        public UnityEventInt OnAttack;

		public UnityEvent OnDeath;
        public UnityEventInt OnHealthChanged;
	    public UnityEventInt OnMaxHealthChanged;

	    public UnityEventInt OnCritChanged;

        private float timeToNextAttack = 5;

	    public void Start() {
	        if (CurrentEnemy != null) {
	            CurrentEnemy.SetLevel(1);
                SetEnemyInstance(CurrentEnemy);
	        }
	    }

        void Update() {
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack <= 0) {
                OnAttack.Invoke(CurrentEnemy.CurrentDamage);
                timeToNextAttack = 1f / CurrentEnemy.CurrentAttackSpeed;
            }
        }

        public void SetEnemyInstance( EnemyInstance e ) {
            if (CurrentEnemy != null) {
                CurrentEnemy.OnDeath -= OnDeath.Invoke;
                CurrentEnemy.CurrentHealth.OnValueChange -= OnHealthChanged.Invoke;
            }

			CurrentEnemy = e;
			e.OnDeath += OnDeath.Invoke;
            e.CurrentHealth.OnValueChange += OnHealthChanged.Invoke;

            OnMaxHealthChanged.Invoke(CurrentEnemy.MaxHealth);
            OnHealthChanged.Invoke(CurrentEnemy.CurrentHealth);
            OnCritChanged.Invoke(CurrentEnemy.CurrentCrit);
            timeToNextAttack = 1f / CurrentEnemy.CurrentAttackSpeed;
        }

#if UNITY_EDITOR
		[Header("TESTING")]
		public EnemyData TestEnemy;

		[ContextMenu("Reload Level")]
		public void ReloadLevel() {
			CurrentEnemy.SetLevel(CurrentEnemy.Level);
		}

		[ContextMenu("Set Test Enemy")]
		public void SetTestEnemy() {
			CurrentEnemy = ScriptableObject.CreateInstance<EnemyInstance>();
            CurrentEnemy.Data = TestEnemy;
			CurrentEnemy.SetLevel(1);

            SetEnemyInstance(CurrentEnemy);
		}
#endif
	}
}

