using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour {

    public Animator animator;
    public Animator armAnimator;
    public Animator armourAnimator;

    public UnityEvent StoppedWalking;
    public UnityEvent StartedWalking;
    public UnityEvent Spun;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	public void StartWalking()
    {
        animator.SetBool("Walking", true);
        armAnimator.SetBool("Walking", true);
        armourAnimator.SetBool("Walking", true);
        StartedWalking.Invoke();

    }

    public void StopWalking()
    {
        animator.SetBool("Walking", false);
        armAnimator.SetBool("Walking", false);
        armourAnimator.SetBool("Walking", false);
        StoppedWalking.Invoke();
    }

    public void Spin()
    {
        animator.SetTrigger("Spin");
        armAnimator.SetTrigger("Spin");
        armourAnimator.SetTrigger("Spin");
        StartCoroutine(SpinEnum());
        Spun.Invoke();
    }

    public IEnumerator SpinEnum()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

        while (!asi.IsName("CharacterSpin"))
        {
            //Debug.Log(asi.ToString());
            yield return null;
            asi = animator.GetCurrentAnimatorStateInfo(0);
        }

        while (asi.IsName("CharacterSpin"))
        {
            yield return null;
            asi = animator.GetCurrentAnimatorStateInfo(0);
        }

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Celebrate()
    {
        animator.SetTrigger("Celebrate");
        armAnimator.SetTrigger("Celebrate");
        armourAnimator.SetTrigger("Celebrate");
    }
}
