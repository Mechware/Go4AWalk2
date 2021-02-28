using G4AW2.Combat;
using G4AW2.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour {

    [Obsolete("Singleton")] public static PlayerAnimations Instance;
    public Animator animator;
    public Animator armAnimator;
    public Animator armourAnimator;
    public Animator weaponAnimator;

    private Action celebrationFinished;

    [SerializeField] private PlayerManager _player;

    [SerializeField] private Image _armour;
    [SerializeField] private Image _weapon;
    [SerializeField] private Image _headgear;


    // Use this for initialization
    void Awake () {
        Instance = this;
        animator = GetComponent<Animator>();
	}

    private void Start()
    {
        RefreshGear();

        _player.WeaponChanged += RefreshGear;
        _player.ArmorChanged += RefreshGear;
        _player.HeadgearChanged += RefreshGear;

    }

    private void RefreshGear()
    {
        _weapon.color = Color.white;
        _armour.color = Color.white;
        _headgear.color = Color.white;

        if (_player.Weapon?.Config.Image == null) _weapon.color = Color.clear;
        if (_player.Armor?.Config.Image == null) _armour.color = Color.clear;
        if (_player.Headgear?.Config.Image == null) _headgear.color = Color.clear;

        _weapon.sprite = _player.Weapon?.Config.Image;
        _armour.sprite = _player.Armor?.Config.Image;
        _headgear.sprite = _player.Headgear?.Config.Image;
    }

    [ContextMenu("StartWalking")]
	public void StartWalking()
    {
        animator.SetBool("Walking", true);
        armAnimator.SetBool("Walking", true);
        armourAnimator.SetBool("Walking", true);
        weaponAnimator.SetBool("Walking", true);

    }

    [ContextMenu("StopWalking")]
    public void StopWalking()
    {
        animator.SetBool("Walking", false);
        armAnimator.SetBool("Walking", false);
        armourAnimator.SetBool("Walking", false);
        weaponAnimator.SetBool("Walking", false);
    }

    [ContextMenu("Spin")]
    public void Spin() {
        spinDone = null;
        animator.SetTrigger("Spin");
        armAnimator.SetTrigger("Spin");
        armourAnimator.SetTrigger("Spin");
        weaponAnimator.SetTrigger("Spin");
    }

    private Action spinDone;
    public void Spin(Action spinDone) {
        this.spinDone = spinDone;
        animator.SetTrigger("Spin");
        armAnimator.SetTrigger("Spin");
        armourAnimator.SetTrigger("Spin");
        weaponAnimator.SetTrigger("Spin");
    }

    public void SpinDone(int forceScale = 0)
    {
        Vector3 scale = transform.localScale;
        if (forceScale != 0) scale.x = forceScale;
        else scale.x *= -1;
        transform.localScale = scale;
        spinDone?.Invoke();
    }

    [ContextMenu("Celebrate")]
    public void Celebrate()
    {
        celebrationFinished = null;
        animator.SetTrigger("Celebrate");
        armAnimator.SetTrigger("Celebrate");
        armourAnimator.SetTrigger("Celebrate");
        weaponAnimator.SetTrigger("Celebrate");
    }

    public void Celebrate(Action onFinish) {
        animator.SetTrigger("Celebrate");
        armAnimator.SetTrigger("Celebrate");
        armourAnimator.SetTrigger("Celebrate");
        weaponAnimator.SetTrigger("Celebrate");
        celebrationFinished = onFinish;
    }

    public void CelebrationDone() {
        celebrationFinished?.Invoke();
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        float speed = _player.GetAttackSpeed() / 2f;
        animator.SetFloat("AttackSpeed", speed);
        armAnimator.SetFloat("AttackSpeed", speed);
        armourAnimator.SetFloat("AttackSpeed", speed);
        weaponAnimator.SetFloat("AttackSpeed", speed);

        animator.SetTrigger("Attack");
        armAnimator.SetTrigger("Attack");
        armourAnimator.SetTrigger("Attack");
        weaponAnimator.SetTrigger("Attack");
    }

    [ContextMenu("ResetAttack")]
    public void ResetAttack()
    {
        animator.ResetTrigger("Attack");
        armAnimator.ResetTrigger("Attack");
        armourAnimator.ResetTrigger("Attack");
        weaponAnimator.ResetTrigger("Attack");
    }
}
