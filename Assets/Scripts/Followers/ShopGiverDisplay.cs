using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;
using UnityEngine.Events;

public class ShopGiverDisplay : MonoBehaviour {

    public ShopUI Shop;

    private ShopFollower follower;

    public void SetData(FollowerData follower) {
        this.follower = (ShopFollower) follower;
        gameObject.SetActive(true);

        Vector2 r = ((RectTransform) transform).sizeDelta;
        r.x = follower.SizeOfSprite.x;
        r.y = follower.SizeOfSprite.y;
        ((RectTransform) transform).sizeDelta = r;
    }

    public void StartWalking() {
        GetComponent<Animator>().SetBool("Walking", true);
    }

    public void StopWalking() {
        GetComponent<Animator>().ResetTrigger("Random");
        GetComponent<Animator>().SetBool("Walking", false);
    }

    public void OnClick() {
        Shop.OpenShop(follower);
    }
}
