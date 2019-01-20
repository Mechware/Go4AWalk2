using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFightingLogic : MonoBehaviour {

	public Player Player;
	public EnemyDisplay EnemyDisplay;
	public float MinSwipeDistance;
    public float FailedParryStunTime = 0.5f;
    public float BlockTime = 1f;
    public float BlockingDamageModifier = 0.5f;
    public float PerfectBlockDamageModifier = 0;
    public float BadParryDamageModifier = 2f;

    public UnityEvent OnBlockStart;
    public UnityEvent OnBlockEnd;
    public UnityEvent OnFailedParry;
    public UnityEvent OnFailedParryDone;
    public UnityEvent OnSuccessfulParry;

    private bool AbleToAttack { get { return !(blocking || perfectBlock || badParry); } }
    private bool blocking = false;
	private bool perfectBlock = false;
	private bool badParry = false;

	public void OnEnemyHitPlayer(int damage) {

        if (perfectBlock)
            damage = Mathf.CeilToInt(damage * PerfectBlockDamageModifier);
        else if (blocking)
            damage = Mathf.CeilToInt(damage * BlockingDamageModifier);
        else if(badParry)
            damage = Mathf.CeilToInt(damage * BadParryDamageModifier);


        Player.Health.Value -= damage;
	}

	public void PlayerAttemptToHitEnemy() {
        if(AbleToAttack)
        {
            EnemyDisplay.ApplyDamage(Player.Damage);
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

	public void PlayerBlocked() {
		blocking = true;

		if (EnemyDisplay.EnemyState == EnemyDisplay.State.BeforeAttack) {
			perfectBlock = true;
		}
        OnBlockStart.Invoke();

        Timer.StartTimer(this, BlockTime, () =>
        {
            blocking = false;
            perfectBlock = false;
            OnBlockEnd.Invoke();
        });
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
