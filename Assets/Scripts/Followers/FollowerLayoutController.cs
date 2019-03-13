using G4AW2.Followers;
using UnityEngine;

public class FollowerLayoutController : MonoBehaviour {

    // Use this for initialization
    void Awake() {
        transform.hasChanged = true;
    }

    public float StartX = 16;
    public float StartY = 32;
    public float XGap = 32;
    public float YGap = 0;

    // Update is called once per frame
    void Update() {
        if(transform.hasChanged) {
            transform.hasChanged = false;
            ChangeLayout();
        }
    }


    [ContextMenu("Apply Layout")]
    public void ChangeLayout() {

        if (transform.childCount == 0) return;

        float x = StartX + 16 - transform.GetChild(transform.childCount-1).GetComponent<FollowerDisplay>().Data.SizeOfSprite.x / 2;
        float y = StartY;

        for(int i = transform.childCount-1; i >= 0; i--) {
            RectTransform child = (RectTransform) transform.GetChild(i);
            Vector3 pos = child.anchoredPosition;
            pos.x = x;
            pos.y = y;
            child.anchoredPosition = pos;
            x += XGap;
            y += YGap;
        }
    }
}
