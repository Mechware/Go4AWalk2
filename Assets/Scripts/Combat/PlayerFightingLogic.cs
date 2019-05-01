using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFightingLogic : MonoBehaviour {

	public Player Player;
	public EnemyDisplay EnemyDisplay;
	public FloatReference MinSwipeDistance;
    public float FailedParryStunTime = 2f;

    public UnityEvent OnBlockStart;
    public UnityEvent OnPerfectBlock;
    public UnityEvent OnBlockEnd;
    public UnityEvent OnFailedParry;
    public UnityEvent OnFailedParryDone;
    public UnityEvent OnSuccessfulParry;
    public UnityEvent Attacked;
    public UnityEventInt AttackHit;

    public DamageNumberSpawner PlayerDamageNumberSpawner;
    public Color DamageColor;

    private bool AbleToAttack { get { return !(blocking || perfectBlock || badParry); } }
    private bool blocking = false;
	private bool perfectBlock = false;
	private bool badParry = false;

	public void OnEnemyHitPlayer(int damage) {

        float fdamage = Player.Armor.Value.GetDamage(damage, perfectBlock, blocking, badParry);

        if(perfectBlock || blocking)
        {
            if(perfectBlock)
                OnPerfectBlock.Invoke();

            blockTimer.Stop();
            blocking = false;
            perfectBlock = false;
            OnBlockEnd.Invoke();
        }

        damage = Mathf.RoundToInt(fdamage);
        Player.DamagePlayer(damage);
	    AttackHit.Invoke(damage);
	    PlayerDamageNumberSpawner.SpawnNumber(damage, DamageColor);

    }

    public float StunnedDamageMultiplier = 1.15f;

	public void PlayerAttemptToHitEnemy() {
        if(AbleToAttack)
        {
            Attacked.Invoke();

            float damageMultipler = EnemyDisplay.EnemyState == EnemyDisplay.State.Stun ? StunnedDamageMultiplier : 1;

            EnemyDisplay.ApplyDamage(Mathf.RoundToInt(Player.GetLightDamage() * damageMultipler), false);
            if(Player.Weapon.Value.IsEnchanted)
                EnemyDisplay.ApplyDamage(Player.GetElementalDamage(), true, Player.Weapon.Value.Enchantment.Type);
            Player.Weapon.Value.TapsWithWeapon.Value++;
        }
    }

	public void PlayerScreenSwipe( Vector3[] swipe ) {
        if (!AbleToAttack) return;
        if (swipe.Length < 2) return;

		Vector3 start = swipe[0];
		Vector3 end = swipe[swipe.Length - 1];
		if ((start - end).magnitude > MinSwipeDistance) {
			if(start.x > end.x) {
				PlayerParried();
			} else {
				PlayerBlocked();
			}
		}
	}

    void Update() {
        blockTimer.Update(Time.deltaTime);
    }

    public float MaxBlockDuration = 4f;

    private UpdateTimer blockTimer = new UpdateTimer();

	public void PlayerBlocked() {
		blocking = true;

	    blockTimer.Start(MaxBlockDuration, () => {
	        blocking = false;
	        perfectBlock = false;
	        OnBlockEnd.Invoke();
	    }, null);


        if (EnemyDisplay.EnemyState == EnemyDisplay.State.BeforeAttack) {
			perfectBlock = true;
		}
        OnBlockStart.Invoke();
	}

	public void PlayerParried() {
		bool success = EnemyDisplay.AttemptedParry();

        if (!success)
        {
            badParry = true;
            OnFailedParry.Invoke();
            Timer.StartTimer(this, FailedParryStunTime, () =>
            {
                OnFailedParryDone.Invoke();
                badParry = false;
            });
        } else
        {
            OnSuccessfulParry.Invoke();
        }
    }
}
