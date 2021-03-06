using G4AW2.Combat;
using G4AW2.Managers;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFighterDisplay : MonoBehaviour {

    public Action<float, ElementalType> OnHit;

    public UnityEvent OnBlockStart;
    public UnityEvent OnPerfectBlock;
    public UnityEvent OnBlockEnd;
    public UnityEvent OnFailedParry;
    public UnityEvent OnFailedParryDone;
    public UnityEvent OnSuccessfulParry;

    [SerializeField] private float _minSwipeDistance = 3f;
    [SerializeField] private float _failedParryStunTime = 1.25f;
    [SerializeField] private float _maxBlockDuration = 4f;
    [SerializeField] private AttackArea _attackArea;

    private float _nextAttackTime = 0;

    private bool _AbleToAttack => _blockState == BlockState.None && Time.time > _nextAttackTime;
    private UpdateTimer _blockTimer = new UpdateTimer();
    private BlockState _blockState = BlockState.None;

    [SerializeField] private EnemyDisplay _enemy;
    [SerializeField] private PlayerManager _player;

    private void Awake()
    {
        _attackArea.OnTap += PlayerAttemptToHitEnemy;
        _attackArea.OnSwipeFinished += PlayerScreenSwipe;
    }

    public void Hit(int damage, ElementalType type) {

        float fdamage;
        if(type == null)
        {
            fdamage = _player.Armor.GetDamage(damage, _blockState);

            if (_blockState == BlockState.Blocking || _blockState == BlockState.PerfectlyBlocking)
            {
                if (_blockState == BlockState.PerfectlyBlocking)
                    OnPerfectBlock.Invoke();

                _blockTimer.Stop();
                OnBlockEnd.Invoke();
                _blockState = BlockState.None;
            }
        } 
        else
        {
            fdamage = _player.Armor.Config.ElementalWeakness[type] * damage;
        }

        damage = Mathf.RoundToInt(fdamage);
        _player.DamagePlayer(damage, type);
    }

	public void PlayerAttemptToHitEnemy() {
        if (!_AbleToAttack) return;

        _nextAttackTime = Time.time + 1f / _player.GetAttackSpeed();

        OnHit?.Invoke(_player.GetLightDamage(), null);
        if(_player.Weapon.IsEnchanted)
            OnHit?.Invoke(_player.GetElementalDamage(), _player.Weapon.Enchantment.Config.Type);

        _player.Weapon.IncrementTaps();
    }

	public void PlayerScreenSwipe( Vector3[] swipe ) {
        if (!_AbleToAttack) return;
        if (swipe.Length < 2) return;

		Vector3 start = swipe[0];
		Vector3 end = swipe[swipe.Length - 1];
		if ((start - end).magnitude > _minSwipeDistance) {
			if(start.x > end.x) {
				PlayerParried();
			} else {
				PlayerBlocked();
			}
		}
	}

    void Update() {
        _blockTimer.Update(Time.deltaTime);
    }

	public void PlayerBlocked() {

	    _blockTimer.Start(_maxBlockDuration, () => {
            _blockState = BlockState.None;
	        OnBlockEnd.Invoke();
	    }, null);

        if (_enemy.EnemyState == EnemyDisplay.State.BeforeAttack) {
            _blockState = BlockState.PerfectlyBlocking;
        }
        OnBlockStart.Invoke();
	}

	public void PlayerParried() {
		bool success = _enemy.EnemyState == EnemyDisplay.State.ExecuteAttack;

        if (!success)
        {
            _blockState = BlockState.BadParry;
            OnFailedParry.Invoke();
            Timer.StartTimer(_failedParryStunTime, () =>
            {
                OnFailedParryDone.Invoke();
                _blockState = BlockState.None;
            }, this);
        } else
        {
            OnSuccessfulParry.Invoke();
        }
    }
}
