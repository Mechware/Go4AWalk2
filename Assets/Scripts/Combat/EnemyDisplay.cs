using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace G4AW2.Combat {
	public class EnemyDisplay : MonoBehaviour {

		public EnemyInstance CurrentEnemy;
		public UnityEvent OnDeath;

		public void SetEnemyInstance( EnemyInstance e ) {
			if (CurrentEnemy != null) CurrentEnemy.OnDeath -= CallOnDeath;
			CurrentEnemy = e;
			e.OnDeath += CallOnDeath;
		}



		private void CallOnDeath() {
			OnDeath.Invoke();
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
		}
#endif
	}
}

