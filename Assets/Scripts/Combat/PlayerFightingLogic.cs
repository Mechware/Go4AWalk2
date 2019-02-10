using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFightingLogic : MonoBehaviour {

	public Player Player;
	public EnemyDisplay EnemyDisplay;
	public float MinSwipeDistance;
    public float FailedParryStunTime = 2f;

    public UnityEvent OnBlockStart;
    public UnityEvent OnBlockEnd;
    public UnityEvent OnFailedParry;
    public UnityEvent OnFailedParryDone;
    public UnityEvent OnSuccessfulParry;
    public UnityEvent Attacked;

    private bool AbleToAttack { get { return !(blocking || perfectBlock || badParry); } }
    private bool blocking = false;
	private bool perfectBlock = false;
	private bool badParry = false;

	public void OnEnemyHitPlayer(int damage) {

        float fdamage = Player.Armor.Value.GetDamage(damage, perfectBlock, blocking, badParry);

        if(perfectBlock || blocking)
        {
            blocking = false;
            perfectBlock = false;
            OnBlockEnd.Invoke();
        }

        damage = Mathf.RoundToInt(fdamage);
        Player.DamagePlayer(damage);
        Debug.Log("Hit for: " + damage);
	}

	public void PlayerAttemptToHitEnemy() {
        if(AbleToAttack)
        {
            Attacked.Invoke();
            EnemyDisplay.ApplyDamage(Player.GetLightDamage());
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
