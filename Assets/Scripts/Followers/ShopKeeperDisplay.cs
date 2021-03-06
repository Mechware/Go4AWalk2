using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopKeeperDisplay : MonoBehaviour, IPointerClickHandler {

    [Obsolete("Do this through events?")]
    public ShopUI Shop;

    public Action StartedWalking;
    public Action FinishInteraction;

    private ShopFollowerInstance followerInstance;

    public void SetData(ShopFollowerInstance follower) {

        RectTransform rt = (RectTransform) transform;

        Vector3 scale = rt.localScale;
        scale.x = 1;
        rt.localScale = scale;

        Vector2 pivot = rt.pivot;
        pivot.x = 1;
        rt.pivot = pivot;

        followerInstance = follower;
        gameObject.SetActive(true);

        Vector2 r = ((RectTransform) transform).sizeDelta;
        r.x = follower.Config.SizeOfSprite.x;
        r.y = follower.Config.SizeOfSprite.y;
        ((RectTransform) transform).sizeDelta = r;

        Vector3 pos = transform.localPosition;
        pos.x = -54;
        transform.localPosition = pos;

        AnimatorOverrideController aoc =
            (AnimatorOverrideController) GetComponent<Animator>().runtimeAnimatorController;

        aoc["Idle"] = followerInstance.Config.SideIdleAnimation;
        aoc["Walk Up"] = followerInstance.Config.WalkingAnimation;
        aoc["Random"] = followerInstance.Config.RandomAnimation;
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
        Shop.OpenShop(followerInstance, () => {

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
