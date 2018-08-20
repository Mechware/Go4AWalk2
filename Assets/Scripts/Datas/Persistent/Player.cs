using System.Collections;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Utils;
using G4AW2.Variables;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player")]
public class Player : ScriptableObject {

    public int MaxHealth;

    public IntVariable Health;
    public IntVariable Crit;
    public int Damage;

    public int GetLightDamage() {
        int crit = Crit;
        crit = crit > Random.Range(0, 100) ? 0 : Mathf.Min(crit + 5, 100);
        Crit.Value = crit;
        return Damage;
    }

    public int GetHeavyDamage( Vector3[] points ) {
        int temp_crit = Crit;
        Crit.Value = 0;
        return Damage * temp_crit / 10;
    }

    public void Hit(int damage) {
        Health.Value -= damage;
    }

#if UNITY_EDITOR
    [ContextMenu("Restore Health")]
    private void ResetHealth() {
        Health.Value = MaxHealth;
    }
#endif
}
