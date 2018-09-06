using CustomEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player")]
public class Player : ScriptableObject {

    public IntReference MaxHealth;

    public IntReference Health;
    public FloatReference Power;
    public IntReference Damage;

	public FloatReference CritResetModifier;
	public FloatReference CritPerHit;

	public void OnEnable() {
		MaxHealth.Value = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
		Health.Value = PlayerPrefs.GetInt("PlayerHealth", 100);
		Power.Value = PlayerPrefs.GetInt("PlayerPower", 0);
		Damage.Value = PlayerPrefs.GetInt("PlayerDamage", 1);
	}

	public void OnDisable() {
		PlayerPrefs.SetInt("PlayerMaxHealth", MaxHealth.Value);
		PlayerPrefs.SetInt("PlayerHealth", Health.Value);
		PlayerPrefs.SetFloat("PlayerPower", Power.Value);
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
		Power.Value = ShouldReset() ? 0 : Mathf.Min(CritPerHit + Power, 100);
	}

	public float CritResetStart = 0.3f;
	public float ResetScale = 16000f;
	private bool ShouldReset() {
		return Random.Range(CritResetStart, 1 + CritResetStart) * CritResetModifier <= (Power * Power / ResetScale);
	}

#if UNITY_EDITOR
	[ContextMenu("Restore Health")]
    private void ResetHealth() {
        Health.Value = MaxHealth;
    }
#endif
}
