using System;
using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;
using BlockState = G4AW2.Data.DropSystem.Armor.BlockState;
using G4AW2.Dialogue;

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

    private BlockState _blockState = BlockState.None;
    private float _nextAttackTime = 0;
    private bool _AbleToAttack => _blockState == Armor.BlockState.None && Time.time > _nextAttackTime;
    private UpdateTimer _blockTimer = new UpdateTimer();
    
    private EnemyDisplay _enemy;
    private Player _player;

    public void Initialize(EnemyDisplay enemy, Player p)
    {
        _enemy = enemy;
        _player = p;
    }

    public void Hit(int damage, ElementalType type) {

        float fdamage;
        if(type == null)
        {
            fdamage = _player.Armor.Value.GetDamage(damage, _blockState);

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
            fdamage = _player.Armor.Value.ElementalWeakness.Value[type] * damage;
        }

        damage = Mathf.RoundToInt(fdamage);
        _player.DamagePlayer(damage, type);
    }

	public void PlayerAttemptToHitEnemy() {
        if (!_AbleToAttack) return;

        _nextAttackTime = Time.time + 1f / _player.GetAttackSpeed();

        OnHit?.Invoke(_player.GetLightDamage(), null);
        if(_player.Weapon.Value.IsEnchanted)
            OnHit?.Invoke(_player.GetElementalDamage(), _player.Weapon.Value.Enchantment.Type);

        _player.Weapon.Value.TapsWithWeapon.Value++;
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
        _blockState = Armor.BlockState.Blocking;

	    _blockTimer.Start(_maxBlockDuration, () => {
            _blockState = Armor.BlockState.None;
	        OnBlockEnd.Invoke();
	    }, null);

        if (_enemy.EnemyState == EnemyDisplay.State.BeforeAttack) {
            _blockState = Armor.BlockState.PerfectlyBlocking;
        }
        OnBlockStart.Invoke();
	}

	public void PlayerParried() {
		bool success = _enemy.EnemyState == EnemyDisplay.State.ExecuteAttack;

        if (!success)
        {
            _blockState = Armor.BlockState.BadParry;
            OnFailedParry.Invoke();
            Timer.StartTimer(_failedParryStunTime, () =>
            {
                OnFailedParryDone.Invoke();
                _blockState = Armor.BlockState.None;
            }, this);
        } else
        {
            OnSuccessfulParry.Invoke();
        }
    }
}
