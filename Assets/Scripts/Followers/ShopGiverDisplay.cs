using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopGiverDisplay : MonoBehaviour, IPointerClickHandler {

    public ShopUI Shop;
    public Action StartedWalking;
    public Action FinishInteraction;

    private ShopFollower follower;

    public void SetData(FollowerData follower) {

        RectTransform rt = (RectTransform) transform;

        Vector3 scale = rt.localScale;
        scale.x = 1;
        rt.localScale = scale;

        Vector2 pivot = rt.pivot;
        pivot.x = 1;
        rt.pivot = pivot;

        this.follower = (ShopFollower) follower;
        gameObject.SetActive(true);

        Vector2 r = ((RectTransform) transform).sizeDelta;
        r.x = follower.SizeOfSprite.x;
        r.y = follower.SizeOfSprite.y;
        ((RectTransform) transform).sizeDelta = r;

        Vector3 pos = transform.localPosition;
        pos.x = -54;
        transform.localPosition = pos;

        AnimatorOverrideController aoc =
            (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;

        aoc["Idle"] = this.follower.SideIdleAnimation;
        aoc["Walk Up"] = this.follower.WalkingAnimation;
        aoc["Random"] = this.follower.RandomAnimation;
    }

    public void StartWalking() {
        StopAllCoroutines();
        GetComponent<Animator>().SetBool("Walking", true);
        StartedWalking.Invoke();
    }

    public void StopWalking() {
        GetComponent<Animator>().ResetTrigger("Random");
        GetComponent<Animator>().SetBool("Walking", false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Shop.OpenShop(follower, () => {

            // Flip Shopper
            RectTransform rt = (RectTransform) transform;
            Vector3 scale = rt.localScale;
            scale.x = -1;
            rt.localScale = scale;

            Vector2 pivot = rt.pivot;
            pivot.x = 0;
            rt.pivot = pivot;

            FinishInteraction.Invoke();

            StartCoroutine(WalkOffScreen());
        });
        
    }

    [Header("Walk Off Parameters")]
    [SerializeField] private float End;
    [SerializeField] private float WalkOffSpeed;
    private bool _walkingOff;
    public void OnScroll(float distance)
    {
        if (!_walkingOff) return;

        RectTransform rt = (RectTransform)transform;
        Vector3 pos = rt.localPosition;
        pos.x -= distance;
        rt.localPosition = pos;
    }

    IEnumerator WalkOffScreen() {

        GetComponent<Animator>().SetBool("Walking", true);

        RectTransform rt = (RectTransform) transform;
        _walkingOff = true;
        while (true) {

            Vector3 pos = rt.localPosition;
            pos.x -= WalkOffSpeed * Time.deltaTime;
            rt.localPosition = pos;

            if (pos.x < End) break;

            yield return null;
        }
        _walkingOff = false;
    }
}
