using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player")]
public class Player : ScriptableObject {

    public IntReference MaxHealth;

    public IntReference Health;
    public FloatReference Crit;
    public IntReference Damage;

	public FloatReference CritResetModifier;
	public FloatReference CritPerHit;

	public void OnEnable() {
		MaxHealth.Value = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
		Health.Value = PlayerPrefs.GetInt("PlayerHealth", 100);
		Crit.Value = PlayerPrefs.GetInt("PlayerCrit", 0);
		Damage.Value = PlayerPrefs.GetInt("PlayerDamage", 1);
	}

	public void OnDisable() {
		PlayerPrefs.SetInt("PlayerMaxHealth", MaxHealth.Value);
		PlayerPrefs.SetInt("PlayerHealth", Health.Value);
		PlayerPrefs.SetFloat("PlayerCrit", Crit.Value);
		PlayerPrefs.SetInt("PlayerDamage", Damage.Value);
	}

    public int GetLightDamage() {
		SetCritValue();
		return Damage;
    }

    public int GetHeavyDamage( float totalCritUsed ) {
        return Damage * Mathf.CeilToInt(totalCritUsed / 10);
    }

    public void Hit(int damage) {
        Health.Value -= damage;
    }

	private void SetCritValue() {
		Crit.Value = ShouldReset() ? 0 : Mathf.Min(CritPerHit + Crit, 100);
	}

	public float CritResetStart = 0.3f;
	public float ResetScale = 16000f;
	private bool ShouldReset() {
		return Random.Range(CritResetStart, 1 + CritResetStart) * CritResetModifier <= (Crit * Crit / ResetScale);
	}

#if UNITY_EDITOR
	[ContextMenu("Restore Health")]
    private void ResetHealth() {
        Health.Value = MaxHealth;
    }
#endif
}
