using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour {

    public Animator animator;
    public Animator armAnimator;
    public Animator armourAnimator;
    public Animator weaponAnimator;

    public UnityEvent StoppedWalking;
    public UnityEvent StartedWalking;
    public UnityEvent Spun;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
    [ContextMenu("StartWalking")]
	public void StartWalking()
    {
        animator.SetBool("Walking", true);
        armAnimator.SetBool("Walking", true);
        armourAnimator.SetBool("Walking", true);
        weaponAnimator.SetBool("Walking", true);
        StartedWalking.Invoke();

    }

    [ContextMenu("StopWalking")]
    public void StopWalking()
    {
        animator.SetBool("Walking", false);
        armAnimator.SetBool("Walking", false);
        armourAnimator.SetBool("Walking", false);
        weaponAnimator.SetBool("Walking", false);
        StoppedWalking.Invoke();
    }

    [ContextMenu("Spin")]
    public void Spin()
    {
        animator.SetTrigger("Spin");
        armAnimator.SetTrigger("Spin");
        armourAnimator.SetTrigger("Spin");
        weaponAnimator.SetTrigger("Spin");
        
        Spun.Invoke();
    }

    public void SpinDone()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    [ContextMenu("Celebrate")]
    public void Celebrate()
    {
        animator.SetTrigger("Celebrate");
        armAnimator.SetTrigger("Celebrate");
        armourAnimator.SetTrigger("Celebrate");
        weaponAnimator.SetTrigger("Celebrate");
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
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
