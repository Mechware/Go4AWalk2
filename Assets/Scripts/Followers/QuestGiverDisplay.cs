using G4AW2.Component.UI;
using G4AW2.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestGiverDisplay : MonoBehaviour, IPointerClickHandler {

    public Action StartedWalking;
    public Action FinishInteraction;

    private QuestGiverInstance follower;

    public void SetData(QuestGiverInstance follower) {

        RectTransform rt = (RectTransform) transform;

        Vector3 scale = rt.localScale;
        scale.x = 1;
        rt.localScale = scale;

        Vector2 pivot = rt.pivot;
        pivot.x = 1;
        rt.pivot = pivot;

        this.follower = follower;
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

        aoc["Idle"] = follower.Config.SideIdleAnimation;
        aoc["Walk Up"] = follower.Config.WalkingAnimation;
        aoc["Random"] = follower.Config.RandomAnimation;
        aoc["QuestGiving"] = follower.Config.GivingQuest;
    }

    public void StartWalking() {
        StopAllCoroutines();
        GetComponent<Animator>().SetBool("Walking", true);
        StartedWalking.Invoke();
    }

    public void StopWalking() {
        GetComponent<Animator>().ResetTrigger("Random");
        GetComponent<Animator>().SetBool("Walking", false);
        GetComponent<Animator>().SetBool("Giving", true);
    }

    public GameObject DismissButton;

    public void OnPointerClick(PointerEventData eventData) {

        PopUp.SetPopUp("Accept quest from quest giver? Title: " + follower.QuestToGive.DisplayName, new[] { "Yes", "No" }, new Action[] {
            () => {
                QuestManager.Instance.GiveQuest(follower.QuestToGive);                
                FollowerManager.Instance.Followers.Remove(follower);

                // Flip Giver
                RectTransform rt = (RectTransform) transform;
                Vector3 scale = rt.localScale;
                scale.x = -1;
                rt.localScale = scale;

                Vector2 pivot = rt.pivot;
                pivot.x = 0;
                rt.pivot = pivot;

                FinishInteraction.Invoke();

                StartCoroutine(WalkOffScreen());

            }, () => {
                DismissButton.SetActive(true);
            } });

    }

    public void Dismiss() {
        FinishInteraction.Invoke();
        FollowerManager.Instance.Followers.Remove(follower);
        StartCoroutine(WalkOffScreen());
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

        GetComponent<Animator>().SetBool("Giving", false);
        GetComponent<Animator>().SetBool("Walking", true);

        RectTransform rt = (RectTransform) transform;

        _walkingOff = true;
        while(true) {

            Vector3 pos = rt.localPosition;
            pos.x -= WalkOffSpeed * Time.deltaTime;
            rt.localPosition = pos;

            if(pos.x < End)
                break;

            yield return null;
        }
        _walkingOff = false;
    }
}
