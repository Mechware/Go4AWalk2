using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using G4AW2.Combat;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour {

    public static PlayerAnimations Instance;
    public Animator animator;
    public Animator armAnimator;
    public Animator armourAnimator;
    public Animator weaponAnimator;

    private Action celebrationFinished;

    // Use this for initialization
    void Awake () {
        Instance = this;
        animator = GetComponent<Animator>();
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

    private double dmg = 0;
    private double elemdmg = 0;
    private ElementalType elemType = null;
    private Action hitCallback;
    [ContextMenu("Attack")]
    public void Attack(double damage, double elemDamage, ElementalType type, Action onHit) {
        dmg = damage;
        elemdmg = elemDamage;
        elemType = type;
        hitCallback = onHit;
        
        animator.SetTrigger("Attack");
        armAnimator.SetTrigger("Attack");
        armourAnimator.SetTrigger("Attack");
        weaponAnimator.SetTrigger("Attack");
    }

    public void OnHit() {
        EnemyDisplay.Instance.RegularDamageNumberSpawner.SpawnNumber(dmg, EnemyDisplay.Instance.BaseDamageColor);
        if(elemType != null) EnemyDisplay.Instance.ElementalDamageNumberSpawner.SpawnNumber(elemdmg, elemType.DamageColor);
        hitCallback?.Invoke();
    }

    [ContextMenu("ResetAttack")]
    public void ResetAttack()
    {
        animator.ResetTrigger("Attack");
        armAnimator.ResetTrigger("Attack");
        armourAnimator.ResetTrigger("Attack");
        weaponAnimator.ResetTrigger("Attack");
    }

    public void SetAttackSpeed(float speed) {
        animator.SetFloat("AttackSpeed", speed);
        armAnimator.SetFloat("AttackSpeed", speed);
        armourAnimator.SetFloat("AttackSpeed", speed);
        weaponAnimator.SetFloat("AttackSpeed", speed);
    }
}
